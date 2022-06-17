using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

[SlashCommandGroup("queue", "Manage the track queue")]
public class SoundtrackQueueCommand : ApplicationCommandModule
{
    private readonly ILogger<SoundtrackQueueCommand> _logger;

    public SoundtrackQueueCommand(ILogger<SoundtrackQueueCommand> logger)
    {
        _logger = logger;
    }

    [SlashCommand("list", "View the current track queue.")]
    public async Task QueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Queue Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Queue Command"));
    }

    [SlashCommand("clear", "Clear out the current track queue")]
    public async Task ClearCommand(InteractionContext context)
    {
        _logger.LogInformation("Clear Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Clear Command"));
    }
}