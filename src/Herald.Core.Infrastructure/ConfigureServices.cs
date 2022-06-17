using Herald.Core.Application.Abstractions;
using Herald.Core.Infrastructure.Persistence;
using Herald.Core.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Herald.Core.Infrastructure;

public static class ConfigureServices
{
    public static IServiceCollection AddHeraldInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<HeraldDbContext>(options =>
            options.UseSqlServer(config.GetConnectionString("DefaultConnection"),
                builder => builder.MigrationsAssembly(typeof(HeraldDbContext).Assembly.FullName)),
            ServiceLifetime.Transient, ServiceLifetime.Transient);

        services.AddTransient<IHeraldDbContext>(provider => provider.GetRequiredService<HeraldDbContext>());
        services.AddScoped<HeraldDbInitializer>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}