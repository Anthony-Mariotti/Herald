using Herald.Core.Domain.Entities.AnyDeals;

namespace Herald.Bot.AnyDeal.Abstractions;

public interface IHeraldAnyDeal
{
    Task<AnyDealDataList> FindAsync(string title);
}
