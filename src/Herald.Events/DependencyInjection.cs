using Herald.Events.Abstractions.Handlers;
using Herald.Events.Handlers;
using Microsoft.Extensions.DependencyInjection;

namespace Herald.Events;

public static class DependencyInjection
{
    public static IServiceCollection AddHeraldEvents(this IServiceCollection services)
        => services
            .AddSingleton<IGuildEventHandler, GuildEventHandler>()
            .AddSingleton<IMessageEventHandler, MessageEventHandler>()
            .AddSingleton<IChannelEventHandler, ChannelEventHandler>();
}