using DSharpPlus.SlashCommands;
using Herald.Bot.Reward.Abstractions;
using Herald.Core.Application.Modules.Queries;
using Herald.Core.Domain.ValueObjects.Modules;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Reward;

public class HeraldReward : IHearaldReward
{
    private readonly ILogger<HeraldReward> _logger;
    private readonly ISender _mediator;

    public HeraldReward(ILogger<HeraldReward> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public Task AddRewardAsync(InteractionContext context) => throw new NotImplementedException();
    public async Task<bool> EnabledAsync(InteractionContext context)
    {
        return await _mediator.Send(new ModuleEnabledQuery(context.Guild.Id, HeraldModule.Reward));
    }
    public Task LeaderboardAsync(InteractionContext context) => throw new NotImplementedException();
    public Task RemoveRewardAsync(InteractionContext context) => throw new NotImplementedException();
}
