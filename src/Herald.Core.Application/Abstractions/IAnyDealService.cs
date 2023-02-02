using Herald.Core.Domain.Entities.AnyDeals;

namespace Herald.Core.Application.Abstractions;

public interface IAnyDealService
{
    Task<AnyDealDataList> SearchAsync(string name);
}
