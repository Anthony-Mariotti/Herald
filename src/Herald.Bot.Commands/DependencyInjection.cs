using Microsoft.Extensions.DependencyInjection;

namespace Herald.Bot.Commands;

public static class DependencyInjection
{
    public static IServiceCollection AddHeraldCommands(this IServiceCollection services) => services;
}