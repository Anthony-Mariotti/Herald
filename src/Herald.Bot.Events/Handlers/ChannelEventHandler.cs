using DSharpPlus;
using DSharpPlus.EventArgs;
using Herald.Bot.Events.Abstractions.Handlers;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Events.Handlers;

public class ChannelEventHandler : IChannelEventHandler
{
    private readonly ILogger<ChannelEventHandler> _logger;
    
    public ChannelEventHandler(ILogger<ChannelEventHandler> logger)
    {
        _logger = logger;
    }

    public Task OnChannelCreated(DiscordClient client, ChannelCreateEventArgs args)
    {
        _logger.LogDebug("Channel Created Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnChannelUpdated(DiscordClient client, ChannelUpdateEventArgs args)
    {
        _logger.LogDebug("Channel Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnChannelDeleted(DiscordClient client, ChannelDeleteEventArgs args)
    {
        _logger.LogDebug("Channel Deleted Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnChannelPinsUpdated(DiscordClient client, ChannelPinsUpdateEventArgs args)
    {
        _logger.LogDebug("Channel Pins Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }
}