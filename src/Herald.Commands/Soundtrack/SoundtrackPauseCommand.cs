using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using Herald.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Herald.Commands.Soundtrack;

public class SoundtrackPauseCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackPauseCommand> _logger;

    public SoundtrackPauseCommand(ILoggerFactory logger) : base(logger)
    {
        _logger = logger.CreateLogger<SoundtrackPauseCommand>();
    }

    [SlashCommand("pause", "Pause the currently playing track.")]
    public async Task PauseCommand(InteractionContext context)
    {
        _logger.LogInformation("Pause Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);

        try
        {
            await LoadLavalinkExtension(context);
            await LoadLavalinkNode();
        }
        catch (LavalinkException e)
        {
            _logger.LogError("Failure loading lavalink node connection. {ErrorMessage}", e.Message);
            await SendErrorConnectionResponse(context);
            return;
        }
        
        if (NodeConnection is null)
        {
            _logger.LogError("Failure loading lavalink node connection");
            await SendErrorConnectionResponse(context);
            return;
        }

        var connection = NodeConnection.GetGuildConnection(context.Guild);

        if (connection is null || 
            !connection.IsConnected || 
            connection.CurrentState.CurrentTrack is null)
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("Not Connected")
                .WithContent("Herald is not currently playing anything."));
            return;
        }

        await connection.PauseAsync();
        
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Pause Command"));
    }
}