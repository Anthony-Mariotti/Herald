using DSharpPlus.Entities;
using DSharpPlus.Lavalink;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Herald.Core.Application.Guilds.Queries.GetGuildModuleStatus;
using Herald.Core.Domain.ValueObjects.Modules;
using Herald.Core.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackBaseCommand : ApplicationCommandModule
{
    private readonly ILogger<SoundtrackBaseCommand> _logger;
    
    protected ISender Mediator { get; }

    private LavalinkExtension? LavalinkExtension { get; set; }

    protected LavalinkNodeConnection NodeConnection { get; set; } = default!;

    protected LavalinkGuildConnection GuildConnection { get; set; } = default!;

    protected SoundtrackBaseCommand(ILoggerFactory logger, ISender mediator)
    {
        Mediator = mediator;
        _logger = logger.CreateLogger<SoundtrackBaseCommand>();
    }

    protected async Task<bool> CommandPreCheckAsync(InteractionContext context)
    {
        if (!await IsModuleEnabled(context)) return false;
        
        try
        {
            await LoadLavalinkExtension(context);
            await LoadLavalinkNode();
            LoadGuildConnection(context);
        }
        catch (LavalinkException e)
        {
            _logger.LogError("Failure loading lavalink node connection. {ErrorMessage}", e.Message);
            await SendErrorConnectionResponse(context);
            return false;
        }

        return true;
    }

    protected async Task<bool> ConnectToChannelAsync(BaseContext context)
    {
        if (context.Member.VoiceState?.Channel is null)
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("Invalid usage")
                .WithContent("You are not in a voice channel."));
            return false;
        }
        
        GuildConnection = NodeConnection.GetGuildConnection(context.Guild);

        if (GuildConnection is not null && GuildConnection.IsConnected)
        {
            var userIsInSameChannel = GuildConnection.Channel.Id.Equals(context.Member.VoiceState.Channel.Id);

            if (userIsInSameChannel) return userIsInSameChannel;
            
            if (!context.Channel.Users.Any()) return userIsInSameChannel;
            
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("Invalid usage")
                .WithContent(
                    $"Join the {GuildConnection.Channel.Name} to listen to music! I'm already in here."));

            return false;
        };
        
        GuildConnection = await NodeConnection.ConnectAsync(context.Member.VoiceState.Channel);
        
        return true;

    }

    private void LoadGuildConnection(BaseContext context)
    {
        GuildConnection = NodeConnection.GetGuildConnection(context.Guild);
    }

    private async Task<bool> IsModuleEnabled(BaseContext context)
    {
        var status = await Mediator.Send(new GetGuildModuleStatusQuery
        {
            GuildId = context.Guild.Id,
            Module = HeraldModule.Soundtrack
        });

        if (!status)
        {
            await context.CreateResponseAsync(HeraldEmbedBuilder
                .Error()
                .WithTitle("Module disabled!")
                .WithDescription("The Soundtrack module is not enabled in this server.")
                .Build()
            );
        }
        
        return status;
    }
    
    private Task LoadLavalinkExtension(BaseContext context)
    {
        LavalinkExtension = context.Client.GetLavalink();

        if (LavalinkExtension.ConnectedNodes.Any()) return Task.CompletedTask;

        throw new LavalinkException("Failure loading lavalink extensions, no connected nodes");

    }

    private Task LoadLavalinkNode()
    {
        if (LavalinkExtension is null)
        {
            throw new LavalinkException($"Failure loading lavalink node connection, lavalink extension is null. Did you forget to call '{nameof(LoadLavalinkExtension)}'");
        }
        
        NodeConnection = LavalinkExtension.ConnectedNodes.Values.First();

        if (NodeConnection is not null) return Task.CompletedTask;

        throw new LavalinkException("Failure loading lavalink node connection");
    }

    private static Task SendErrorConnectionResponse(BaseContext context)
    {
        return context.CreateResponseAsync(HeraldEmbedBuilder
            .Error()
            .WithTitle("Soundtrack error")
            .WithDescription("I'm having trouble connecting to music services at this time.")
            .Build());
    }
}