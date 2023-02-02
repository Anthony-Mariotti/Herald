using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.AnyDeals;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Application.AnyDeals.Queries.FindAnyDeal;
public record FindAnyDealQuery(string Title) : IRequest<AnyDealDataList>;

public class FindAnyDealQueryHandler : IRequestHandler<FindAnyDealQuery, AnyDealDataList>
{
    private readonly ILogger<FindAnyDealQueryHandler> _logger;
    private readonly IAnyDealService _anyDealService;

    public FindAnyDealQueryHandler(ILogger<FindAnyDealQueryHandler> logger, IAnyDealService anyDealService)
    {
        _logger = logger;
        _anyDealService = anyDealService;
    }

    public async Task<AnyDealDataList> Handle(FindAnyDealQuery request, CancellationToken cancellationToken)
    {
        _logger.LogDebug("Handling {Command}: {@Request}", nameof(FindAnyDealQuery), new
        {
            request.Title,
        });
        var result = await _anyDealService.SearchAsync(request.Title);

        return result is null 
           ? new AnyDealDataList()
           : result;
    }
}
