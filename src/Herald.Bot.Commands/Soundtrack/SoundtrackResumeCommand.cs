using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Lavalink4NET;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackResumeCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackResumeCommand> _logger;
    
    public SoundtrackResumeCommand(ILoggerFactory logger, IAudioService audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackResumeCommand>();
    }
    
    [SlashCommand("resume", "Resume a paused track")]
    public async Task ResumeCommand(InteractionContext context)
    {
        _logger.LogInformation("Soundtrack Resume Command Executed by {User} in {Guild}", context.Guild.Name, context.User.Username);

        if (!await CommandPreCheckAsync(context))
            return;

        var player = await GetPlayerAsync(context);

        await player.ResumeAsync();

        await context.CreateResponseAsync(ResumeEmbed(context.Member));
    }
    
    private static DiscordEmbed ResumeEmbed(DiscordUser user)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Music has resumed")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
}