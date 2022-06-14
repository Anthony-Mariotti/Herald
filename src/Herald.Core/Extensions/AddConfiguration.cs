using Herald.Core.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Herald.Core.Extensions;

public static partial class HeraldCoreExtensions
{
    public static IServiceCollection AddConfiguration(this IServiceCollection services)
        => services.AddSingleton(provider =>
        {
            var config = provider.GetRequiredService<IConfiguration>();
            return config.GetSection(nameof(HeraldConfig)).Get<HeraldConfig>();
        });
}