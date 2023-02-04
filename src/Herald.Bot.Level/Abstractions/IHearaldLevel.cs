using DSharpPlus.SlashCommands;

namespace Herald.Bot.Level.Abstractions;

public interface IHeraldReward
{
    Task<bool> EnabledAsync(InteractionContext context);
    
    Task AddRewardAsync(InteractionContext context);

    Task RemoveRewardAsync(InteractionContext context);

    Task LeaderboardAsync(InteractionContext context);
}
