using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackSkipCommand : ApplicationCommandModule
{
    private readonly ILogger<SoundtrackSkipCommand> _logger;

    public SoundtrackSkipCommand(ILogger<SoundtrackSkipCommand> logger)
    {
        _logger = logger;
    }

    [SlashCommand("skip", "Skip the current track that is currently playing")]
    public async Task SkipCommand(InteractionContext context)
    {
        _logger.LogInformation("Skip Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Skip Command"));
    }
}