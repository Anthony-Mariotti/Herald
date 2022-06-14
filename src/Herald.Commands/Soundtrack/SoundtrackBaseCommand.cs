using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using Herald.Core.Exceptions;
using Microsoft.Extensions.Logging;

namespace Herald.Commands.Soundtrack;

public class SoundtrackBaseCommand : ApplicationCommandModule
{
    private readonly ILogger<SoundtrackBaseCommand> _logger;
    
    protected LavalinkExtension? LavalinkExtension { get; set; }
    
    protected LavalinkNodeConnection? NodeConnection { get; set; }

    protected SoundtrackBaseCommand(ILoggerFactory logger)
    {
        _logger = logger.CreateLogger<SoundtrackBaseCommand>();
    }
    
    protected virtual Task LoadLavalinkExtension(InteractionContext context)
    {
        LavalinkExtension = context.Client.GetLavalink();

        if (LavalinkExtension.ConnectedNodes.Any()) return Task.CompletedTask;

        throw new LavalinkException("Failure loading lavalink extensions, no connected nodes");

    }

    protected virtual Task LoadLavalinkNode()
    {
        if (LavalinkExtension is null)
        {
            throw new LavalinkException($"Failure loading lavalink node connection, lavalink extension is null. Did you forget to call '{nameof(LoadLavalinkExtension)}'");
        }
        
        NodeConnection = LavalinkExtension.ConnectedNodes.Values.First();

        if (NodeConnection is not null) return Task.CompletedTask;

        throw new LavalinkException("Failure loading lavalink node connection");
    }

    protected static Task SendErrorConnectionResponse(InteractionContext context)
    {
        return context.CreateResponseAsync(new DiscordInteractionResponseBuilder()
            .WithTitle("Soundtrack Error")
            .WithContent("I'm having trouble connecting to music services at this time."));
    }
}