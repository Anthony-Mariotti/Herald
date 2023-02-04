using Herald.Core.Application.Abstractions;
using Herald.Core.Infrastructure.Common.Configuration;
using Herald.Core.Infrastructure.Common.Exceptions;
using Herald.Core.Infrastructure.Persistence;
using Herald.Core.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using System.Diagnostics.CodeAnalysis;

namespace Herald.Core.Infrastructure.Common.Extensions;

public static partial class InfrastructureExtensions
{
    [SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "Readability")]
    public static IServiceCollection AddHeraldInfrastructure(this IServiceCollection services, IConfiguration configuration, bool production = true)
    {
        var dbConfig = configuration.GetSection("Database").Get<DatabaseConfig>();

        if (dbConfig is null)
        {
            throw new HeraldInfrastructureException("Could not find database configuration");
        }

        services.AddDbContext<IHeraldDbContext, HeraldDbContext>(options =>
        {
            var builder = options.UseMySql(
                connectionString: dbConfig.ConnectionString,
                serverVersion: ServerVersion.AutoDetect(dbConfig.ConnectionString),
                mySqlOptionsAction: mysqlOptions => mysqlOptions.EnableRetryOnFailure());

            if (!production)
            {
                builder.EnableSensitiveDataLogging()
                    .EnableDetailedErrors()
                    .LogTo(Log.Logger.Debug, LogLevel.Information);
            }

        }, ServiceLifetime.Transient, ServiceLifetime.Transient);

        services.AddScoped<HeraldDbInitializer>();

        services.AddAnyDealService(configuration);
        services.AddDateTimeService();

        return services;
    }
}
