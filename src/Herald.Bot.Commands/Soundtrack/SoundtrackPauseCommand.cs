using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Lavalink4NET;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackPauseCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackPauseCommand> _logger;

    public SoundtrackPauseCommand(ILoggerFactory logger, IAudioService audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackPauseCommand>();
    }

    [SlashCommand("pause", "Pause the currently playing track.")]
    public async Task PauseCommand(InteractionContext context)
    {
        _logger.LogInformation("Pause Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);

        if (!await CommandPreCheckAsync(context)) return;

        var player = await GetPlayerAsync(context);

        await player.PauseAsync();

        await context.CreateResponseAsync(PauseEmbed(context.Member));
    }
    
    private static DiscordEmbed PauseEmbed(DiscordUser user)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Music Paused")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
}