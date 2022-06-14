using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace Herald.Commands.Soundtrack;

public class SoundtrackStopCommand : ApplicationCommandModule
{
    private readonly ILogger<SoundtrackStopCommand> _logger;

    public SoundtrackStopCommand(ILogger<SoundtrackStopCommand> logger)
    {
        _logger = logger;
    }

    [SlashCommand("stop", "Stop playing the current track and disconnects the bot from the voice channel.")]
    public async Task StopCommand(InteractionContext context)
    {
        _logger.LogInformation("Stop Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Stop Command"));
    }
}