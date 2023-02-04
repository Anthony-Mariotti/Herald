using Herald.Bot.AnyDeal.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace Herald.Bot.AnyDeal;

public static partial class ConfigureServices
{
    public static IServiceCollection AddHeraldAnyDeal(this IServiceCollection services)
    {
        _ = services.AddScoped<IHeraldAnyDeal, HeraldAnyDeal>();
        return services;
    }
}
