using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Herald.Core.Application.Soundtracks.Commands.PlayTrackCommand;
using Herald.Core.Application.Soundtracks.Queries.GetNextTrack;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackSkipCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackSkipCommand> _logger;

    public SoundtrackSkipCommand(ILoggerFactory logger, ISender mediator)
        : base(logger, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackSkipCommand>();
    }

    [SlashCommand("skip", "Skip the current track that is currently playing")]
    public async Task SkipCommand(InteractionContext context)
    {
        _logger.LogInformation("Skip Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);

        if (!await CommandPreCheckAsync(context))
            return;
        
        if (!GuildConnection.IsConnected)
        {
            await context.CreateResponseAsync(
                new DiscordInteractionResponseBuilder().WithContent(
                    "I'm not currently connected to any voice channel."));
            return;
        }

        var nextTrack = await Mediator.Send(new GetNextTrackQuery(context.Guild.Id));
        
        if (nextTrack is null)
        {
            if (GuildConnection.CurrentState.CurrentTrack is null)
            {
                await context.CreateResponseAsync(
                    new DiscordInteractionResponseBuilder().WithContent(
                        "There is nothing to skip, I'm not currently playing anything"));
                return;
            } 
            
            await context.CreateResponseAsync(QueueEndedEmbed());
            await GuildConnection.DisconnectAsync();
            return;
        }

        var track = await NodeConnection.Rest.DecodeTrackAsync(nextTrack.TrackString);
        track.TrackString = nextTrack.TrackString;
        
        await GuildConnection.StopAsync();
        await GuildConnection.PlayAsync(track);
        
        await context.CreateResponseAsync(NowPlayingEmbed(track, context.Member));
        
        await Mediator.Send(new PlayTrackCommand
        {
            GuildId = context.Guild.Id,
            Track = track,
            NotifyChannelId = context.Channel.Id,
            RequestUserId = context.Member.Id
        });
    }
    
    private static DiscordEmbed NowPlayingEmbed(LavalinkTrack track, DiscordUser user)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Now Playing", iconUrl: "https://play-lh.googleusercontent.com/SqMGe5wxL6HfT03WNGepMvGxXyS9EOFm4V7NzLCofFxPwFVJqRavYe5-EPQV3WAW7DU")
            .WithTitle(track.Title)
            .WithUrl(track.Uri)
            .WithImageUrl($"https://i.ytimg.com/vi/{track.Identifier}/hq720.jpg")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();

    private static DiscordEmbed QueueEndedEmbed()
        => HeraldEmbedBuilder
            .Information()
            .WithAuthor("Queue Ended")
            .Build();
}