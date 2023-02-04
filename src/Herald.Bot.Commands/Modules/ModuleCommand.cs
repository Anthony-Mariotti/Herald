using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Core.Application.Modules.Commands.ModuleDisable;
using Herald.Core.Application.Modules.Commands.ModuleEnable;
using Herald.Core.Domain.Entities.Modules;
using Herald.Core.Utility;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Modules;

public enum GuildModules
{
    [ChoiceName("Soundtrack")]
    Soundtrack,
    [ChoiceName("Economy")]
    Economy
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
    public async Task ModulesList(InteractionContext context)
    {
        _logger.LogInformation(
            "{Command} command executed by {User} in {Guild}",
            nameof(ModulesList),
            context.User.Id,
            context.Guild.Id);

        try
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: List of modules"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {Command} slash command", nameof(ModulesList));
        }
    }

    [SlashCommand("enable", "Enable a module on the server.")]
    public async Task ModuleEnable(
        InteractionContext context,
        [Option("module", "Module to enable")] GuildModules modules)
    {
        _logger.LogInformation(
             "{Command} command executed by {User} in {Guild}",
             nameof(ModuleEnable),
             context.User.Id,
             context.Guild.Id);

        try
        {
            var module = Module.From(modules.ToString());

            if (module.Id == -1)
            {
                await context.CreateResponseAsync(
                    HeraldEmbedBuilder.Error().WithTitle($"{modules} is an invalid module..."));
                return;
            }

            _ = await _mediator.Send(new ModuleEnableCommand
            {
                GuildId = context.Guild.Id,
                Module = module
            });

            await context.CreateResponseAsync(
                HeraldEmbedBuilder.Success().WithTitle($"The {modules} module is now enabled!").Build());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {Command} slash command", nameof(ModuleEnable));
        }
    }

    [SlashCommand("disable", "Disable a module on the server.")]
    public async Task ModuleDisable(
        InteractionContext context,
        [Option("module", "Module to disable")] GuildModules modules)
    {
        _logger.LogInformation(
            "{Command} command executed by {User} in {Guild}",
            nameof(ModuleDisable),
            context.User.Id,
            context.Guild.Id);

        try
        {
            var module = Module.From(modules.ToString());

            if (module.Id == -1)
            {
                await context.CreateResponseAsync(
                    HeraldEmbedBuilder.Error().WithTitle($"{modules} is an invalid module..."));
                return;
            }

            _ = await _mediator.Send(new ModuleDisableCommand
            {
                GuildId = context.Guild.Id,
                Module = module
            });

            await context.CreateResponseAsync(
                HeraldEmbedBuilder.Success().WithTitle($"The {modules} is now disabled!").Build());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {Command} slash command", nameof(ModuleDisable));
        }
    }
}