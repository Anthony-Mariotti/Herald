using DSharpPlus.SlashCommands;
using Herald.Bot.Level.Abstractions;
using Herald.Core.Application.Modules.Queries;
using Herald.Core.Domain.Entities.Modules;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Level;

public class HeraldLevel : IHeraldReward
{
    private readonly ISender _mediator;

    public HeraldLevel(ISender mediator)
    {
        _mediator = mediator;
    }

    public Task AddRewardAsync(InteractionContext context) => throw new NotImplementedException();
    public async Task<bool> EnabledAsync(InteractionContext context) =>
        await _mediator.Send(new ModuleEnabledQuery(context.Guild.Id, Module.Economy));
    public Task LeaderboardAsync(InteractionContext context) => throw new NotImplementedException();
    public Task RemoveRewardAsync(InteractionContext context) => throw new NotImplementedException();
}
