using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackQueueCommand : ApplicationCommandModule
{
    private readonly ILogger<SoundtrackQueueCommand> _logger;

    public SoundtrackQueueCommand(ILogger<SoundtrackQueueCommand> logger)
    {
        _logger = logger;
    }

    [SlashCommand("queue", "View the current track queue.")]
    public async Task QueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Queue Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Queue Command"));
    }
}