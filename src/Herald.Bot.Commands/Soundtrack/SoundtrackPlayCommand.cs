using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.Lavalink.EventArgs;
using DSharpPlus.SlashCommands;
using Herald.Core.Application.Soundtracks.Commands.AddTrackToQueue;
using Herald.Core.Application.Soundtracks.Commands.PlayNextTrack;
using Herald.Core.Application.Soundtracks.Queries.GetQueue;
using Herald.Core.Exceptions;
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

        if (!await IsModuleEnabled(context))
        {
            return;
        }
        
        if (context.Member.VoiceState?.Channel is null)
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("Invalid Usage")
                .WithContent("You are not in a voice channel."));
            return;
        }

        try
        {
            await LoadLavalinkExtension(context);
            await LoadLavalinkNode();
        }
        catch (LavalinkException e)
        {
            _logger.LogError("Failure loading lavalink node connection. {ErrorMessage}", e.Message);
            await SendErrorConnectionResponse(context);
            return;
        }
        
        var trackLoadResult = await SearchForTrack(search);
        
        if (trackLoadResult == null)
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("No Results")
                .WithContent("No matching results where found."));
            return;
        }
        
        if (trackLoadResult.LoadResultType is LavalinkLoadResultType.NoMatches)
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("No Results")
                .WithContent("No matching results where found."));
            return;
        }
        
        LavalinkTrack selectedTrack = default!;

        // TODO: IF YOUTUBE SEARCH SHOW MULTI PROMPT FOR LIST OF TRACKS
        if (trackLoadResult.Tracks.Count() > 1)
        {
            // TODO: SHOW PROMPT
            selectedTrack = trackLoadResult.Tracks.First();
        }

        if (trackLoadResult.Tracks.Count() == 1)
        {
            selectedTrack = trackLoadResult.Tracks.First();
        }

        if (NodeConnection is null)
        {
            _logger.LogError("Failure loading lavalink node connection");
            await SendErrorConnectionResponse(context);
            return;
        }

        var connection = NodeConnection.GetGuildConnection(context.Member.VoiceState.Guild) ??
                         await NodeConnection.ConnectAsync(context.Member.VoiceState.Channel);

        if (connection.CurrentState.CurrentTrack is not null)
        {
            await Mediator.Send(new AddTrackToQueueCommand
            {
                GuildId = context.Guild.Id,
                NotifyChannelId = context.Channel.Id,
                Track = selectedTrack
            });
            
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("Currently Playing")
                .WithContent($"Added to Queue {selectedTrack.Title}"));
            return;
        }
        
        
        connection.PlaybackFinished += PlaybackFinished;
        await connection.PlayAsync(selectedTrack);
        
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("Now Playing the selected track"));
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
        var queue = await Mediator.Send(new GetQueueQuery(connection.Guild.Id));

        if (args.Reason == TrackEndReason.Finished)
        {
            if (queue.Tracks.Any())
            {
                var nextTrack = queue.Tracks.First();

                if (NodeConnection is null)
                {
                    await DisconnectAsync(connection);
                    return;
                }
            
                var track = await NodeConnection.Rest.DecodeTrackAsync(nextTrack.TrackString);

                await connection.PlayAsync(track);
                await Mediator.Send(new PlayNextTrackCommand
                {
                    GuildId = connection.Guild.Id,
                    TrackIdentifier = track.Identifier
                });

                var responseChannel = connection.Guild.GetChannel(queue.NotifyChannelId);
                await responseChannel.SendMessageAsync($"Now playing {track.Title} ({track.Length:hh\\:mm\\:ss})!");
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
}