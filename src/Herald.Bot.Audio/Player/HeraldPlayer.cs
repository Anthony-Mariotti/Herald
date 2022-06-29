using DSharpPlus;
using Herald.Core.Application.Soundtracks.Commands.PlayTrackFromQueue;
using Herald.Core.Application.Soundtracks.Commands.TrackEnded;
using Herald.Core.Application.Soundtracks.Queries.GetNextTrack;
using Herald.Core.Domain.Enums;
using Lavalink4NET.Decoding;
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

        var endedTrack = TrackDecoder.DecodeTrack(eventArgs.TrackIdentifier);
        
        switch (eventArgs.Reason)
        {
            case TrackEndReason.Finished:
                _logger.LogTrace("Running Track Finished for {GuildId}", GuildId);
                await _mediator.Send(new TrackEndedCommand
                {
                    GuildId = GuildId,
                    Identifier = endedTrack.TrackIdentifier,
                    Reason = TrackStatusReason.TrackEnded
                });

                if (eventArgs.MayStartNext)
                {
                    if (await PlayNextTrack()) return;

                    if (DisconnectOnStop)
                        await DisconnectAsync();
                }
                break;
            case TrackEndReason.LoadFailed:
                _logger.LogTrace("Running Loading Failed for {GuildId}", GuildId);
                await _mediator.Send(new TrackEndedCommand
                {
                    GuildId = GuildId,
                    Identifier = endedTrack.TrackIdentifier,
                    Reason = TrackStatusReason.TrackFailed
                });
                
                if (eventArgs.MayStartNext)
                {
                    if (await PlayNextTrack()) return;

                    if (DisconnectOnStop)
                        await DisconnectAsync();
                }
                break;
            case TrackEndReason.Stopped:
                _logger.LogTrace("Running Track Stopped for {GuildId}", GuildId);
                await _mediator.Send(new TrackEndedCommand
                {
                    GuildId = GuildId,
                    Identifier = endedTrack.TrackIdentifier,
                    Reason = TrackStatusReason.UserStopped
                });
                
                if (DisconnectOnStop)
                    await DisconnectAsync();
                break;
            case TrackEndReason.Replaced:
                _logger.LogTrace("Running Track Replaced for {GuildId}", GuildId);
                await _mediator.Send(new TrackEndedCommand
                {
                    GuildId = GuildId,
                    Identifier = endedTrack.TrackIdentifier,
                    Reason = TrackStatusReason.UserSkipped
                });

                if (eventArgs.MayStartNext)
                {
                    if (await PlayNextTrack()) return;

                    if (DisconnectOnStop)
                        await DisconnectAsync();
                }
                break;
            case TrackEndReason.CleanUp:
                _logger.LogTrace("Running Track CleanUp for {GuildId}", GuildId);
                await _mediator.Send(new TrackEndedCommand
                {
                    GuildId = GuildId,
                    Identifier = endedTrack.TrackIdentifier,
                    Reason = TrackStatusReason.TrackCleanUp
                });
                break;
            default:
#pragma warning disable CA2208
                throw new ArgumentOutOfRangeException(nameof(eventArgs.Reason));
#pragma warning restore CA2208
        }
    }

    private async Task<bool> PlayNextTrack()
    {
        var nextTrack = await _mediator.Send(new GetNextTrackQuery(GuildId));

        if (nextTrack is null) return false;
        
        _logger.LogTrace("Starting next track {TrackIdentifier} on {GuildId}", nextTrack.Identifier, GuildId);
        var track = nextTrack.GetLavalinkTrack();
                
        var channel = await _client.GetChannelAsync(nextTrack.NotifyChannelId);
        var user = await _client.GetUserAsync(nextTrack.RequestUserId);

        await _client.SendMessageAsync(channel, HeraldAudioMessage.NowPlayingEmbed(track, user));
        await PlayAsync(track);
        await _mediator.Send(new PlayTrackFromQueueCommand
        {
            GuildId = GuildId,
            TrackIdentifier = nextTrack.Identifier
        });
        return true;

    }
}