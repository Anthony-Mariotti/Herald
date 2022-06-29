using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Core.Application.Modules.Commands.ModuleDisable;
using Herald.Core.Application.Modules.Commands.ModuleEnable;
using Herald.Core.Domain.ValueObjects.Modules;
using Herald.Core.Utility;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Module;

public enum GuildModules
{
    [ChoiceName("Soundtrack")]
    Soundtrack
}

[SlashCommandGroup("modules", "Manage the bot modules for this server.")]
public class ModuleCommand : ApplicationCommandModule
{
    private readonly ILogger<ModuleCommand> _logger;
    private readonly ISender _mediator;

    public ModuleCommand(ILogger<ModuleCommand> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    [SlashCommand("list", "List of all of the available modules.")]
    public async Task ListModulesCommand(InteractionContext context)
    {
        _logger.LogInformation("Module List Command Executed by {User} in {Guild}", context.Guild.Name, context.User.Username);
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: List of modules"));
    }

    [SlashCommand("enable", "Enable a module on the server.")]
    public async Task EnableModuleCommand(
        InteractionContext context,
        [Option("module", "Module to enable")] GuildModules modules)
    {
        _logger.LogInformation("Module Enable Command Executed by {User} in {Guild}", context.Guild.Name, context.User.Username);

        await _mediator.Send(new ModuleEnableCommand
        {
            GuildId = context.Guild.Id,
            Module = HeraldModule.From(modules.ToString())
        });

        await context.CreateResponseAsync(
            HeraldEmbedBuilder.Success().WithTitle($"The {modules} is now enabled!").Build());
    }

    [SlashCommand("disable", "Disable a module on the server.")]
    public async Task DisableModule(
        InteractionContext context,
        [Option("module", "Module to disable")] GuildModules modules)
    {
        _logger.LogInformation("Module Disable Command Executed by {User} in {Guild}", context.Guild.Name, context.User.Username);

        await _mediator.Send(new ModuleDisableCommand
        {
            GuildId = context.Guild.Id,
            Module = HeraldModule.From(modules.ToString())
        });
        
        await context.CreateResponseAsync(
            HeraldEmbedBuilder.Success().WithTitle($"The {modules} is now disabled!").Build());
    }
}