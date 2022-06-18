using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Herald.Core.Application.Soundtracks.Commands.PlayTrackCommand;
using Herald.Core.Application.Soundtracks.Commands.TrackEnded;
using Herald.Core.Application.Soundtracks.Queries.GetNextTrack;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

[SlashCommandGroup("queue", "Manage the track queue")]
public class SoundtrackQueueCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackQueueCommand> _logger;

    public SoundtrackQueueCommand(ILoggerFactory logger, ISender mediator)
        : base(logger, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackQueueCommand>();
    }

    [SlashCommand("list", "View the current track queue.")]
    public async Task QueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Queue Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Queue Command"));
    }

    [SlashCommand("clear", "Clear out the current track queue")]
    public async Task ClearQueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Clear Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Clear Command"));
    }

    [SlashCommand("play", "Start playing tracks from the queue")]
    public async Task PlayQueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Play Queue Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);

        if (!await CommandPreCheckAsync(context))
            return;

        if (GuildConnection?.CurrentState?.CurrentTrack is not null)
        {
            // TODO: Already playing the track queue.
            return;
        }
        
        if (GuildConnection?.Channel is null)
        {
            await ConnectToChannelAsync(context);
        }

        var nextTrack = await Mediator.Send(new GetNextTrackQuery(context.Guild.Id));

        if (nextTrack is null)
        {
            // TODO: No more tracks to play
            return;
        }

        var track = await NodeConnection.Rest.DecodeTrackAsync(nextTrack.TrackString);

        if (track is null)
        {
            // TODO: Error decoding track
            return;
        }

        track.TrackString = nextTrack.TrackString;

        if (GuildConnection is not null)
        {
            GuildConnection.PlaybackFinished += PlaybackFinished;
            await GuildConnection.PlayAsync(track);
        
            await context.CreateResponseAsync(NowPlayingEmbed(track));

            await Mediator.Send(new PlayTrackCommand
            {
                GuildId = context.Guild.Id,
                Track = track,
                NotifyChannelId = context.Channel.Id,
                RequestUserId = context.User.Id
            });
        }
    }

    [SlashCommand("add", "Add tracks to the queue")]
    public async Task QueueAddCommand(InteractionContext context)
    {
        _logger.LogInformation("Queue Add Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Queue Add Command"));
    }
    
    private static DiscordEmbed NowPlayingEmbed(LavalinkTrack track)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Playing from Queue", iconUrl: "https://play-lh.googleusercontent.com/SqMGe5wxL6HfT03WNGepMvGxXyS9EOFm4V7NzLCofFxPwFVJqRavYe5-EPQV3WAW7DU")
            .WithTitle(track.Title)
            .WithUrl(track.Uri)
            .WithImageUrl($"https://i.ytimg.com/vi/{track.Identifier}/hq720.jpg")
            .WithFooter($"Requested by User", "https://cdn.discordapp.com/avatars/105522177406672896/0ca616626c74ae17f3a901ef45dab1bf.webp?size=32")
            .WithTimestamp(DateTime.Now)
            .Build();
    
    private async Task PlaybackFinished(LavalinkGuildConnection connection, TrackFinishEventArgs args)
    {
        var nextTrack = await Mediator.Send(new GetNextTrackQuery(connection.Guild.Id));
        
        if (args.Reason == TrackEndReason.Finished)
        {
            _logger.LogDebug("Playback finished for {TrackValue} in {GuildId}", args.Track.Identifier, args.Player.Guild.Id);
            if (nextTrack is not null)
            {
                var track = await NodeConnection.Rest.DecodeTrackAsync(nextTrack.TrackString);
                track.TrackString = nextTrack.TrackString;
        
                await connection.PlayAsync(track);
        
                // TODO: Reply to original message from channel
                var channel = connection.Guild.GetChannel(nextTrack.NotifyChannelId);
        
                if (channel is null)
                {
                    _logger.LogWarning("Unable to find channel {ChannelId} in {GuildId}", nextTrack.NotifyChannelId, connection.Guild.Id);
                }
                
                channel?.SendMessageAsync(NowPlayingEmbed(track));
        
                await Mediator.Send(new TrackEndedCommand
                {
                    GuildId = connection.Guild.Id,
                    TrackId = args.Track.Identifier
                });
                
                await Mediator.Send(new PlayTrackCommand
                {
                    GuildId = connection.Guild.Id,
                    Track = track,
                    NotifyChannelId = nextTrack.NotifyChannelId,
                    RequestUserId = nextTrack.RequestUserId
                });
                return;
            }
            
            await Mediator.Send(new TrackEndedCommand
            {
                GuildId = connection.Guild.Id,
                TrackId = args.Track.Identifier
            });
            
            await DisconnectAsync(connection);
        }
    }
    
    private async Task DisconnectAsync(LavalinkGuildConnection connection)
    {
        connection.PlaybackFinished -= PlaybackFinished;
        await connection.DisconnectAsync();
    }
}