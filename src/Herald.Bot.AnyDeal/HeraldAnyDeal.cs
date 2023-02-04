using Herald.Bot.AnyDeal.Abstractions;
using Herald.Core.Application.AnyDeals.Queries.FindAnyDeal;
using Herald.Core.Domain.Entities.AnyDeals;
using MediatR;

namespace Herald.Bot.AnyDeal;

public class HeraldAnyDeal : IHeraldAnyDeal
{
    private readonly ISender _mediator;

    public HeraldAnyDeal(ISender mediator)
    {
        _mediator = mediator;
    }

    public async Task<AnyDealDataList> FindAsync(string title)
    {
        var result = await _mediator.Send(new FindAnyDealQuery(title));
        return result;
    }
}
