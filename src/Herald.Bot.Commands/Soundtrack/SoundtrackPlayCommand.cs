using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Lavalink4NET;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackPlayCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackPlayCommand> _logger;

    public SoundtrackPlayCommand(ILoggerFactory logger, IAudioService audio, ISender mediator)
        : base(logger, audio, mediator)
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

        var selectedTrack = await AudioService.GetTrackAsync(search, SearchMode.YouTube);
        
        if (selectedTrack is null)
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("No Results")
                .WithContent("No matching results where found."));
            return;
        }

        var player = await GetPlayerAsync(context);
        
        await context.CreateResponseAsync(NowPlayingEmbed(selectedTrack));
        
        await player.PlayAsync(selectedTrack, true, context.Member.Id, context.Channel.Id);
    }

    private static DiscordEmbed NowPlayingEmbed(LavalinkTrack track)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Now Playing", iconUrl: "https://play-lh.googleusercontent.com/SqMGe5wxL6HfT03WNGepMvGxXyS9EOFm4V7NzLCofFxPwFVJqRavYe5-EPQV3WAW7DU")
            .WithTitle(track.Title)
            .WithUrl(track.Source)
            .WithImageUrl($"https://i.ytimg.com/vi/{track.Identifier}/hq720.jpg")
            .WithFooter($"Requested by User", "https://cdn.discordapp.com/avatars/105522177406672896/0ca616626c74ae17f3a901ef45dab1bf.webp?size=32")
            .WithTimestamp(DateTime.Now)
            .Build();

    private static DiscordEmbed AddedToQueueEmbed(LavalinkTrack track)
        => HeraldEmbedBuilder
             .Success()
             .WithAuthor("Added track to queue!", iconUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3b/Eo_circle_green_checkmark.svg/2048px-Eo_circle_green_checkmark.svg.png")
             .WithTitle(track.Title)
             .WithUrl(track.Source)
             .WithThumbnail($"https://i.ytimg.com/vi/{track.Identifier}/hq720.jpg")
             .WithFooter($"Requested by User", "https://cdn.discordapp.com/avatars/105522177406672896/0ca616626c74ae17f3a901ef45dab1bf.webp?size=32")
             .WithTimestamp(DateTime.Now)
             .Build();
}