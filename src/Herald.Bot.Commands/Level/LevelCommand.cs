using DSharpPlus;
using DSharpPlus.SlashCommands;
using Herald.Bot.Level.Abstractions;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Level;

[SlashCommandGroup("level", "Manage your currency for the server")]
public class LevelCommand : ApplicationCommandModule
{
    private readonly ILogger<LevelCommand> _logger;
    private readonly IHeraldReward _reward;

    public LevelCommand(ILogger<LevelCommand> logger, IHeraldReward reward)
    {
        _logger = logger;
        _reward = reward;
    }

    [SlashCommand("add", "Add a xp points to a particular user")]
    [SlashCommandPermissions(Permissions.Administrator)]
    public async Task RewardAdd(InteractionContext context)
    {
        _logger.LogInformation(
            "{Command} command executed by {User} in {Guild}",
            nameof(RewardAdd),
            context.User.Id,
            context.Guild.Id);
        try
        {
            await _reward.AddRewardAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {Command} slash command", nameof(RewardAdd));
        }
    }

    [SlashCommand("take", "Take xp points from a particular user")]
    [SlashCommandPermissions(Permissions.Administrator)]
    public async Task RewardTake(InteractionContext context)
    {
        _logger.LogInformation(
            "{Command} command executed by {User} in {Guild}",
            nameof(RewardTake),
            context.User.Id,
            context.Guild.Id);
        try
        {
            await _reward.RemoveRewardAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling {Command} slash command", nameof(RewardTake));
        }
    }
}
