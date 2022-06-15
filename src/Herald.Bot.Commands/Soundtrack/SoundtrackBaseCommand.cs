using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using Herald.Core.Application.Modules.Queries.GetModuleStatus;
using Herald.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackBaseCommand : ApplicationCommandModule
{
    private readonly ILogger<SoundtrackBaseCommand> _logger;
    
    protected ISender Mediator { get; }
    
    protected LavalinkExtension? LavalinkExtension { get; set; }
    
    protected LavalinkNodeConnection? NodeConnection { get; set; }

    protected SoundtrackBaseCommand(ILoggerFactory logger, ISender mediator)
    {
        Mediator = mediator;
        _logger = logger.CreateLogger<SoundtrackBaseCommand>();
    }

    protected async Task<bool> IsModuleEnabled(InteractionContext context)
    {
        var status = await Mediator.Send(new GetGuildModuleStatusQuery
        {
            GuildId = context.Guild.Id,
            ModuleName = "Soundtrack"
        });

        if (!status.Enabled)
        {
            await context.CreateResponseAsync(
                new DiscordInteractionResponseBuilder().WithContent(
                    "The Soundtrack module is not enabled in this server."));
        }
        
        return status.Enabled;
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