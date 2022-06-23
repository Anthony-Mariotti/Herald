using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Lavalink4NET;
using Lavalink4NET.Player;
using Lavalink4NET.Rest;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

[SlashCommandGroup("queue", "Manage the track queue")]
public class SoundtrackQueueCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackQueueCommand> _logger;

    public SoundtrackQueueCommand(ILoggerFactory logger, IAudioService audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackQueueCommand>();
    }

    [SlashCommand("list", "View the current track queue.")]
    public async Task QueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Queue Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        
        if (!await CommandPreCheckAsync(context))
            return;
        
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Queue List Command"));
    }

    [SlashCommand("clear", "Clear out the current track queue")]
    public async Task ClearQueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Clear Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        
        if (!await CommandPreCheckAsync(context))
            return;
        
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Queue Clear Command"));
    }

    [SlashCommand("play", "Start playing tracks from the queue")]
    public async Task PlayQueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Play Queue Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);

        if (!await CommandPreCheckAsync(context))
            return;

        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Queue Play Command"));
    }

    [SlashCommand("add", "Add tracks to the queue")]
    public async Task QueueAddCommand(InteractionContext context,
        [Option("search", "Track title or url")] string search = "")
    {
        _logger.LogInformation("Queue Add Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        
        if (!await CommandPreCheckAsync(context))
            return;

        var selectedTrack = await AudioService.GetTrackAsync(search, SearchMode.YouTube);
        
        if (selectedTrack is null)
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("No Results")
                .WithContent("No matching results where found."));
            return;
        }
        
        var player = await GetPlayerAsync(context);

        await player.EnqueueAsync(selectedTrack, context.Member.Id, context.Channel.Id);
        
        await context.CreateResponseAsync(AddedToQueueEmbed(selectedTrack, context.Member));
    }
    
    private static DiscordEmbed AddedToQueueEmbed(LavalinkTrack track, DiscordUser user)
        => HeraldEmbedBuilder
            .Success()
            .WithAuthor("Added track to queue!", iconUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3b/Eo_circle_green_checkmark.svg/2048px-Eo_circle_green_checkmark.svg.png")
            .WithTitle(track.Title)
            .WithUrl(track.Source)
            .WithThumbnail($"https/img.youtube.com/vi/{track.Identifier}/0.jpg")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
    
    private static DiscordEmbed NowPlayingEmbed(LavalinkTrack track)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Playing from Queue", iconUrl: "https://play-lh.googleusercontent.com/SqMGe5wxL6HfT03WNGepMvGxXyS9EOFm4V7NzLCofFxPwFVJqRavYe5-EPQV3WAW7DU")
            .WithTitle(track.Title)
            .WithUrl(track.Source)
            .WithImageUrl($"https/img.youtube.com/vi/{track.Identifier}/0.jpg")
            .WithFooter($"Requested by User", "https://cdn.discordapp.com/avatars/105522177406672896/0ca616626c74ae17f3a901ef45dab1bf.webp?size=32")
            .WithTimestamp(DateTime.Now)
            .Build();
}