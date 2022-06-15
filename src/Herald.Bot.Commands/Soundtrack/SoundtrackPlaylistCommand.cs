using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackPlaylistCommand : ApplicationCommandModule
{
    private readonly ILogger<SoundtrackPlaylistCommand> _logger;

    public SoundtrackPlaylistCommand(ILogger<SoundtrackPlaylistCommand> logger)
    {
        _logger = logger;
    }

    [SlashCommand("playlist", "Add an entire playlist to the track queue.")]
    public async Task PlaylistCommand(InteractionContext context)
    {
        _logger.LogInformation("Playlist Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        await context.CreateResponseAsync(
            new DiscordInteractionResponseBuilder().WithContent("TODO: Playlist Command"));
    }
}