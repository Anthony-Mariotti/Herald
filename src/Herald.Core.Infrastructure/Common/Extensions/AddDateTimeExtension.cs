using Herald.Core.Application.Abstractions;
using Herald.Core.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Herald.Core.Infrastructure.Common.Extensions;

public static partial class InfrastructureExtensions
{
    [SuppressMessage("Style", "IDE0058:Expression value is never used", Justification = "Readability")]
    public static IServiceCollection AddDateTimeService(this IServiceCollection services)
    {
        services.AddTransient<IDateTime, DateTimeService>();
        return services;
    }
}
