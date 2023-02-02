using Herald.Core.Application.Abstractions;
using Herald.Core.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Herald.Core.Infrastructure.Common.Extensions;

public static partial class InfrastructureExtensions
{
    public static IServiceCollection AddDateTimeService(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, DateTimeService>();
        return services;
    }
}
