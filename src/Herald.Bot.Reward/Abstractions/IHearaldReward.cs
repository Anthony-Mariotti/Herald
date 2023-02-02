using DSharpPlus.SlashCommands;

namespace Herald.Bot.Reward.Abstractions;

public interface IHearaldReward
{
    Task<bool> EnabledAsync(InteractionContext context);
    
    Task AddRewardAsync(InteractionContext context);

    Task RemoveRewardAsync(InteractionContext context);

    Task LeaderboardAsync(InteractionContext context);
}
