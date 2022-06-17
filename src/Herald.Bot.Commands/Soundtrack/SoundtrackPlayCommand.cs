using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Herald.Core.Application.Soundtracks.Commands.PlayTrackCommand;
using Herald.Core.Application.Soundtracks.Commands.QueueTrack;
using Herald.Core.Application.Soundtracks.Commands.TrackEnded;
using Herald.Core.Application.Soundtracks.Queries.GetNextTrack;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackPlayCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackPlayCommand> _logger;
    
    public SoundtrackPlayCommand(ILoggerFactory logger, ISender mediator) : base(logger, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackPlayCommand>();
    }
    
    [SlashCommand("play", "Play or add a track from the current queue.")]
    public async Task PlayCommand(
        InteractionContext context,
        [Option("search", "Track title or url")] string search = "")
    {
        _logger.LogInformation("Soundtrack Play Command Executed by {User} in {Guild}", context.Guild.Name, context.User.Username);

        if (!await CommandPreCheckAsync(context)) return;
        
        if (!await ConnectToChannelAsync(context))
            return;

        var trackSearchResult = await SearchForTrack(search);
        
        if (trackSearchResult == null)
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("No Results")
                .WithContent("No matching results where found."));
            return;
        }
        
        if (trackSearchResult.LoadResultType is LavalinkLoadResultType.NoMatches)
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("No Results")
                .WithContent("No matching results where found."));
            return;
        }
        
        var selectedTrack = trackSearchResult.Tracks.First();

        if (GuildConnection.CurrentState.CurrentTrack is not null)
        {
            await context.CreateResponseAsync(AddedToQueueEmbed(selectedTrack));
            
            await Mediator.Send(new QueueTrackCommand
            {
                GuildId = context.Guild.Id,
                NotifyChannelId = context.Channel.Id,
                RequestUserId = context.User.Id,
                Track = selectedTrack
            });
            
            return;
        }

        await context.CreateResponseAsync(NowPlayingEmbed(selectedTrack));
        
        GuildConnection.PlaybackFinished += PlaybackFinished;
        await GuildConnection.PlayAsync(selectedTrack);
        
        await Mediator.Send(new PlayTrackCommand
        {
            GuildId = context.Guild.Id,
            NotifyChannelId = context.Channel.Id,
            RequestUserId = context.User.Id,
            Track = selectedTrack
        });
    }

    private async Task<LavalinkLoadResult?> SearchForTrack(string search)
    {
        LavalinkLoadResult? loadResult = null;

        if (Uri.TryCreate(search, UriKind.Absolute, out var searchUri))
        {
            loadResult = await NodeConnection!.Rest.GetTracksAsync(searchUri);

            if (loadResult.LoadResultType is LavalinkLoadResultType.LoadFailed or LavalinkLoadResultType.NoMatches)
            {
                loadResult = null;
            }
        }

        if (loadResult is not null) return loadResult;
        
        loadResult = await NodeConnection!.Rest.GetTracksAsync(search);
            
        if (loadResult.LoadResultType is LavalinkLoadResultType.LoadFailed or LavalinkLoadResultType.NoMatches)
        {
            loadResult = null;
        }

        return loadResult;
    }
    
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
                    Track = track
                });
                return;
            }
            
            await DisconnectAsync(connection);
        }
    }
    
    private async Task DisconnectAsync(LavalinkGuildConnection connection)
    {
        connection.PlaybackFinished -= PlaybackFinished;
        await connection.DisconnectAsync();
    }

    private static DiscordEmbed NowPlayingEmbed(LavalinkTrack track)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Now Playing", iconUrl: "https://play-lh.googleusercontent.com/SqMGe5wxL6HfT03WNGepMvGxXyS9EOFm4V7NzLCofFxPwFVJqRavYe5-EPQV3WAW7DU")
            .WithTitle(track.Title)
            .WithUrl(track.Uri)
            .WithImageUrl($"https://i.ytimg.com/vi/{track.Identifier}/hq720.jpg")
            .WithFooter($"Requested by User", "https://cdn.discordapp.com/avatars/105522177406672896/0ca616626c74ae17f3a901ef45dab1bf.webp?size=32")
            .WithTimestamp(DateTime.Now)
            .Build();

    private static DiscordEmbed AddedToQueueEmbed(LavalinkTrack track)
        => HeraldEmbedBuilder
             .Success()
             .WithAuthor("Added track to queue!", iconUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3b/Eo_circle_green_checkmark.svg/2048px-Eo_circle_green_checkmark.svg.png")
             .WithTitle(track.Title)
             .WithUrl(track.Uri)
             .WithThumbnail($"https://i.ytimg.com/vi/{track.Identifier}/hq720.jpg")
             .WithFooter($"Requested by User", "https://cdn.discordapp.com/avatars/105522177406672896/0ca616626c74ae17f3a901ef45dab1bf.webp?size=32")
             .WithTimestamp(DateTime.Now)
             .Build();
}