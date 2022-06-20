using Herald.Core.Application.Soundtracks.Commands.PauseTrack;
using Herald.Core.Application.Soundtracks.Commands.PlayTrack;
using Herald.Core.Application.Soundtracks.Commands.QueueTrack;
using Herald.Core.Application.Soundtracks.Commands.ResumeTrack;
using Herald.Core.Application.Soundtracks.Commands.StopTrack;
using Herald.Core.Application.Soundtracks.Queries.GetNextTrack;
using Herald.Core.Application.Soundtracks.Queries.HasQueuedTrackQuery;
using Herald.Core.Domain.ValueObjects.Soundtracks;
using Lavalink4NET.Events;
using Lavalink4NET.Player;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Audio.Player;

public sealed class HeraldPlayer : LavalinkPlayer
{
    private readonly ILogger<HeraldPlayer> _logger;
    private readonly ISender _mediator;

    public HeraldPlayer(ILoggerFactory logger, ISender mediator)
    {
        _logger = logger.CreateLogger<HeraldPlayer>();
        _mediator = mediator;
    }

    public async Task PlayAsync(LavalinkTrack track, bool enqueue, ulong requestUserId, ulong notifyChannelId,
        TimeSpan? startTime = null, TimeSpan? endTime = null, bool noReplace = false)
    {
        EnsureNotDestroyed();
        EnsureConnected();

        var hasQueuedTrack = await _mediator.Send(new HasQueuedTrackQuery(GuildId));

        if (enqueue && (hasQueuedTrack || State is PlayerState.Playing or PlayerState.Paused))
        {
            await EnqueueAsync(track, requestUserId, notifyChannelId);
            return;
        }

        await _mediator.Send(new PlayTrackCommand
        {
            GuildId = GuildId,
            Track = QueuedTrackValue.Create(track, notifyChannelId, requestUserId, true)
        });
        
        await base.PlayAsync(track, startTime, endTime, noReplace);
    }

    public async Task EnqueueAsync(LavalinkTrack track, ulong requestUserId, ulong notifyChannelId)
    {
        EnsureNotDestroyed();
        EnsureConnected();
        
        try
        {
            await _mediator.Send(new QueueTrackCommand
            {
                GuildId = GuildId,
                Track = QueuedTrackValue.Create(track, notifyChannelId, requestUserId)
            });
        }
        catch (Exception)
        {
            _logger.LogError("Failed to enqueue track: {@Track} for {Guild}", track, GuildId);
        }
    }
    
    public async Task SkipAsync(int count = 1, ulong? requestUserId = null, ulong? notifyChannelId = null)
    {
        if (count <= 0)
            return;
        
        EnsureNotDestroyed();
        EnsureConnected();

        // TODO: Skip 'count' number of tracks
        var nextTrack = await _mediator.Send(new GetNextTrackQuery(GuildId));

        if (nextTrack is not null)
        {
            await PlayAsync(nextTrack.GetLavalinkTrack(), false,
                requestUserId ?? nextTrack.RequestUserId,
                notifyChannelId ?? nextTrack.NotifyChannelId);
            return;
        }

        await StopAsync(DisconnectOnStop);
        // TODO: Clear Queue / Mark all tracks as played.
    }

    public override async Task StopAsync(bool disconnect = false)
    {
        EnsureNotDestroyed();
        EnsureConnected();

        try {
            await base.StopAsync(disconnect);
            await _mediator.Send(new StopTrackCommand(GuildId));
        } catch (InvalidOperationException) { }
    }

    public override async Task PauseAsync()
    {
        EnsureNotDestroyed();
        EnsureConnected();

        try
        {
            await base.PauseAsync();
            await _mediator.Send(new PauseTrackCommand(GuildId));
        } catch (InvalidOperationException) { }
    }

    public override async Task ResumeAsync()
    {
        EnsureNotDestroyed();
        EnsureConnected();

        try
        {
            await base.ResumeAsync();
            await _mediator.Send(new ResumeTrackCommand(GuildId));
        } catch (InvalidOperationException) { }
    }

    public override Task OnTrackEndAsync(TrackEndEventArgs eventArgs)
    {
        if (eventArgs.MayStartNext)
        {
            return SkipAsync();
        }

        return DisconnectOnStop
            ? DisconnectAsync()
            : Task.CompletedTask;
    }
}