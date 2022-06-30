using DSharpPlus;
using DSharpPlus.SlashCommands;
using Herald.Bot.Audio.Abstractions;
using Herald.Bot.Audio.Player;
using Herald.Core.Application.Soundtracks.Commands.DequeueTrack;
using Herald.Core.Application.Soundtracks.Commands.PauseTrack;
using Herald.Core.Application.Soundtracks.Commands.PlayTrack;
using Herald.Core.Application.Soundtracks.Commands.PlayTrackFromQueue;
using Herald.Core.Application.Soundtracks.Commands.QueueTrack;
using Herald.Core.Application.Soundtracks.Commands.ResumeTrack;
using Herald.Core.Application.Soundtracks.Queries.GetNextTrack;
using Herald.Core.Application.Soundtracks.Queries.HasQueuedTrack;
using Herald.Core.Domain.Enums;
using Herald.Core.Domain.ValueObjects.Soundtracks;
using Lavalink4NET;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Audio;

public class HeraldAudio : IHeraldAudio
{
    private readonly ILogger<HeraldAudio> _logger;
    private readonly IAudioService _service;
    private readonly DiscordClient _client;
    private readonly ISender _mediator;

    private HeraldPlayer _player;

    public HeraldAudio(ILogger<HeraldAudio> logger, HeraldPlayer player, IAudioService service, DiscordClient client,
        ISender mediator)
    {
        _logger = logger;
        _player = player;
        _service = service;
        _client = client;
        _mediator = mediator;
    }

    public async Task<LavalinkTrack?> SearchAsync(InteractionContext context, string search, SearchMode mode)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (string.IsNullOrWhiteSpace(search))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(search));

        var result = await _service.GetTrackAsync(search, mode);

        return result;
    }

    public async Task PlayAsync(InteractionContext context, string search, SearchMode mode)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (string.IsNullOrWhiteSpace(search))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(search));

        await EnsureGuildHasPlayerAsync(context);

        var result = await SearchAsync(context, search, mode);

        if (result is null)
        {
            await context.CreateResponseAsync(HeraldAudioMessage.TrackNotFoundEmbed(search, context.Member));
            return;
        }

        if (_player.CurrentTrack is not null || _player.State is PlayerState.Playing or PlayerState.Paused)
        {
            await EnqueueAsync(context, result);
            return;
        }

        await context.CreateResponseAsync(HeraldAudioMessage.NowPlayingEmbed(result, context.Member));

        await _mediator.Send(new PlayTrackCommand
        {
            GuildId = context.Guild.Id,
            Track = QueuedTrackValue.Create(result, context.Channel.Id, context.Member.Id, TrackStatus.Playing,
                TrackStatusReason.UserAdded)
        });
        
        await _player.PlayAsync(result);
    }

    public async Task SkipAsync(InteractionContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        await EnsureGuildHasPlayerAsync(context);
        
        var hasNextTrack = await _mediator.Send(new HasQueuedTrackQuery(context.Guild.Id));

        if (!hasNextTrack)
        {
            await _player.DisconnectAsync();
            return;
        }

        var nextTrack = await _mediator.Send(new GetNextTrackQuery(context.Guild.Id));

        if (nextTrack is null)
        {
            await _player.DisconnectAsync();
            return;
        }

        var track = nextTrack.GetLavalinkTrack();
        var user = await _client.GetUserAsync(nextTrack.RequestUserId);
        
        await _player.PlayAsync(nextTrack.GetLavalinkTrack());
        await context.CreateResponseAsync(HeraldAudioMessage.NowPlayingEmbed(track, user));
        await _mediator.Send(new PlayTrackFromQueueCommand
        {
            GuildId = context.Guild.Id,
            TrackIdentifier = nextTrack.Identifier
        });
    }

    public async Task EnqueueAsync(InteractionContext context, string search, SearchMode mode)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));
        if (string.IsNullOrWhiteSpace(search))
            throw new ArgumentException("Value cannot be null or whitespace.", nameof(search));
        
        var result = await SearchAsync(context, search, mode);

        if (result is null)
        {
            await context.CreateResponseAsync(HeraldAudioMessage.TrackNotFoundEmbed(search, context.Member));
            return;
        }

        await EnqueueAsync(context, result, true);
    }

    public async Task EnqueueAsync(InteractionContext context, LavalinkTrack track, bool queueOnly = false)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));
        if (track is null) throw new ArgumentNullException(nameof(track));

        if (!queueOnly)
        {
            await EnsureGuildHasPlayerAsync(context);
        }
        
        // TODO: Has Queued Track
        // var hasTrack = await Mediator.Send(new HasTrackQuery
        // {
        //    GuildId = context.Guild.Id,
        //    Identifier = track.TrackIdentifier
        // });
        
        // if (hasTrack)
        // {
        //     TODO: Has Track Message with Queue Position
        //     return;
        // }
        
        await context.CreateResponseAsync(HeraldAudioMessage.AddedToQueueEmbed(track, context.Member));

        await _mediator.Send(new QueueTrackCommand
        {
            GuildId = context.Guild.Id,
            Track = QueuedTrackValue.Create(track, context.Channel.Id, context.Member.Id, TrackStatus.Queued,
                TrackStatusReason.UserAdded)
        });
    }

    public async Task DequeueAsync(InteractionContext context, QueuedTrackValue track)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));
        if (track is null) throw new ArgumentNullException(nameof(track));

        await EnsureGuildHasPlayerAsync(context);

        await context.CreateResponseAsync(HeraldAudioMessage.TrackDequeuedEmbed(track, context.Member));

        await _mediator.Send(new DequeueTrackCommand(context.Guild.Id, track));
    }

    public Task GetQueueAsync(InteractionContext context)
    {
        throw new NotImplementedException();
    }

    public Task PlayQueueAsync(InteractionContext context)
    {
        // TODO: Check if Guild Has Track Queued
        
        // TODO: Get Queued Track
        
        // TODO: Send Message and Play Track
        
        // TODO: Send Play Track From Queue

        throw new NotImplementedException();
    }

    public async Task StopAsync(InteractionContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        await EnsureGuildHasPlayerAsync(context);

        await _player.StopAsync(true);
        
        await context.CreateResponseAsync(HeraldAudioMessage.StoppedEmbed(context.Member));
    }

    public async Task PauseAsync(InteractionContext context)
    {
        if (context is null) throw new ArgumentNullException(nameof(context));

        await EnsureGuildHasPlayerAsync(context);

        await _player.PauseAsync();
        
        await context.CreateResponseAsync(HeraldAudioMessage.PausedEmbed(context.Member));

        await _mediator.Send(new PauseTrackCommand(context.Guild.Id));
    }

    public async Task ResumeAsync(InteractionContext context)
    {
        if (context == null) throw new ArgumentNullException(nameof(context));

        await EnsureGuildHasPlayerAsync(context);

        await _player.ResumeAsync();

        if (_player.CurrentTrack is null)
        {
            await context.CreateResponseAsync(HeraldAudioMessage.NoTrackToResumeEmbed(context.Member));
            return;
        }
        
        await context.CreateResponseAsync(HeraldAudioMessage.ResumedEmbed(_player.CurrentTrack, context.Member));

        await _mediator.Send(new ResumeTrackCommand(context.Guild.Id));
    }

    private async Task EnsureGuildHasPlayerAsync(BaseContext context)
    {
        if (_service.HasPlayer(context.Guild.Id))
        {
            if (_player.GuildId.Equals(context.Guild.Id))
                return;
            
            _player = _service.GetPlayer<HeraldPlayer>(context.Guild.Id) ??
                      throw new InvalidOperationException("Failed to load player for guild.");
            return;
        }

        if (context.Member.VoiceState.Channel is null)
            throw new ArgumentNullException(nameof(context.Member.VoiceState.Channel), "Must provide a voice channel id to join.");
        
        _player = await _service.JoinAsync(() => _player, context.Guild.Id, context.Member.VoiceState.Channel.Id);
    }
}