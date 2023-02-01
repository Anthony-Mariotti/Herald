using Herald.Core.Application.Abstractions;
using Herald.Core.Infrastructure.Common.Configuration;
using Herald.Core.Infrastructure.Common.Exceptions;
using Herald.Core.Infrastructure.Persistence;
using Herald.Core.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Infrastructure.Common.Extensions;

public static partial class InfrastructureExtensions
{
    public static IServiceCollection AddHeraldInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var dbConfig = configuration.GetSection("Database").Get<DatabaseConfig>();

        if (dbConfig is null)
        {
            throw new HeraldInfrastructureException("Could not find database configuration");
        }

        services.AddDbContext<IHeraldDbContext, HeraldDbContext>(options =>
        {
            options.UseMySql(
                connectionString: dbConfig.ConnectionString,
                serverVersion: ServerVersion.AutoDetect(dbConfig.ConnectionString),
                mySqlOptionsAction: mysqlOptions =>
                {
                    mysqlOptions.EnableRetryOnFailure();
                })
            .EnableSensitiveDataLogging()
            .EnableDetailedErrors()
            .LogTo(Console.WriteLine, LogLevel.Trace);
        }, ServiceLifetime.Transient, ServiceLifetime.Transient);

        services.AddScoped<HeraldDbInitializer>();

        services.AddTransient<IDateTime, DateTimeService>();

        return services;
    }
}
