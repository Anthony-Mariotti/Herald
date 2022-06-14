using Herald.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;

namespace Herald.Core;

public static class DependencyInjection
{
    public static IServiceCollection AddHeraldCore(this IServiceCollection services)
        => services
            .AddConfiguration();
}