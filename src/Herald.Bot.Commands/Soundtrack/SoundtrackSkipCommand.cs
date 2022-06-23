using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Lavalink4NET;
using Lavalink4NET.Player;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackSkipCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackSkipCommand> _logger;

    public SoundtrackSkipCommand(ILoggerFactory logger, IAudioService audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackSkipCommand>();
    }

    [SlashCommand("skip", "Skip the current track that is currently playing")]
    public async Task SkipCommand(InteractionContext context)
    {
        _logger.LogInformation("Skip Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);

        if (!await CommandPreCheckAsync(context))
            return;

        var player = await GetPlayerAsync(context);

        await player.SkipAsync(1, context.Member.Id, context.Channel.Id);

        if (player.CurrentTrack is null)
        {
            await SendErrorResponse(context, "Soundtrack error", "There was an error skipping the track.");
            return;
        }

        await context.CreateResponseAsync(NowPlayingEmbed(player.CurrentTrack, context.Member));
    }
    
    private static DiscordEmbed NowPlayingEmbed(LavalinkTrack track, DiscordUser user)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Now Playing", iconUrl: "https://play-lh.googleusercontent.com/SqMGe5wxL6HfT03WNGepMvGxXyS9EOFm4V7NzLCofFxPwFVJqRavYe5-EPQV3WAW7DU")
            .WithTitle(track.Title)
            .WithUrl(track.Source)
            .WithImageUrl($"https/img.youtube.com/vi/{track.Identifier}/0.jpg")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
}