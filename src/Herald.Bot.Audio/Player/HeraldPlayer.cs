using DSharpPlus;
using Herald.Core.Application.Soundtracks.Commands.PlayTrack;
using Herald.Core.Application.Soundtracks.Commands.TrackEnded;
using Herald.Core.Application.Soundtracks.Queries.GetNextTrack;
using Herald.Core.Domain.Enums;
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
    private readonly DiscordClient _client;

    public HeraldPlayer(ILoggerFactory logger, ISender mediator, DiscordClient client)
    {
        _logger = logger.CreateLogger<HeraldPlayer>();
        _mediator = mediator;
        _client = client;
    }

    public override async Task OnTrackEndAsync(TrackEndEventArgs eventArgs)
    {
        EnsureNotDestroyed();
        EnsureConnected();
        _logger.LogTrace("Track Ended {TrackIdentifier} on {GuildId}", eventArgs.TrackIdentifier, GuildId);

        if (eventArgs.Reason is not TrackEndReason.Stopped && eventArgs.MayStartNext)
        {
            await _mediator.Send(new TrackEndedCommand
            {
                GuildId = GuildId,
                Identifier = CurrentTrack?.TrackIdentifier!
            });
            
            var nextTrack = await _mediator.Send(new GetNextTrackQuery(GuildId));

            if (nextTrack is not null)
            {
                _logger.LogTrace("Starting next track {TrackIdentifier} on {GuildId}", nextTrack.Identifier, GuildId);
                var track = nextTrack.GetLavalinkTrack();
                
                var channel = await _client.GetChannelAsync(nextTrack.NotifyChannelId);
                var user = await _client.GetUserAsync(nextTrack.RequestUserId);

                await _client.SendMessageAsync(channel, HeraldAudioMessage.NowPlayingEmbed(track, user));
                await PlayAsync(track);
                await _mediator.Send(new PlayTrackCommand
                {
                    GuildId = GuildId,
                    Track = QueuedTrackValue.Create(track, nextTrack.NotifyChannelId, nextTrack.RequestUserId,
                        TrackStatus.Playing)
                });
                return;
            }
        }

        if (DisconnectOnStop && eventArgs.Reason is not TrackEndReason.Replaced)
        {
            _logger.LogTrace("Disconnecting on stop for {GuildId}", GuildId);
            await DisconnectAsync();
        }
    }
}