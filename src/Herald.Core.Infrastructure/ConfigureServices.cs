using Herald.Core.Application.Abstractions;
using Herald.Core.Infrastructure.Persistence;
using Herald.Core.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Herald.Core.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddHeraldInfrastructure(this IServiceCollection services)
    {
        services.AddTransient<IHeraldDbContext, HeraldDbContext>();
        services.AddTransient<IDateTime, DateTimeService>();

        services.AddScoped<HeraldDbInitializer>();

        return services;
    }
}