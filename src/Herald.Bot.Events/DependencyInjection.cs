using Herald.Bot.Events.Abstractions.Handlers;
using Herald.Bot.Events.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Herald.Bot.Events;

public static class DependencyInjection
{
    public static IServiceCollection AddHeraldEvents(this IServiceCollection services)
        => services
            .AddSingleton<IGuildEventHandler, GuildEventHandler>()
            .AddSingleton<IMessageEventHandler, MessageEventHandler>()
            .AddSingleton<IChannelEventHandler, ChannelEventHandler>()
            .AddSingleton<IScheduledGuildEventHandler, ScheduledGuildEventHandler>()
            .AddSingleton<IUnknownEventHandler, UnknownEventHandler>();
}