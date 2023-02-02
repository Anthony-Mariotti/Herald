using Herald.Bot.AnyDeal.Abstractions;
using Herald.Core.Application.AnyDeals.Queries.FindAnyDeal;
using Herald.Core.Domain.Entities.AnyDeals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.AnyDeal;

public class HeraldAnyDeal : IHeraldAnyDeal
{
    private readonly ILogger<HeraldAnyDeal> _logger;
    private readonly ISender _mediator;

    public HeraldAnyDeal(ILogger<HeraldAnyDeal> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    public async Task<AnyDealDataList> FindAsync(string title)
    {
        var result = await _mediator.Send(new FindAnyDealQuery(title));
        return result;
    }
}
