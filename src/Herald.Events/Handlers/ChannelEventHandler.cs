﻿using DSharpPlus;
using DSharpPlus.EventArgs;
using Herald.Events.Abstractions.Handlers;
using Microsoft.Extensions.Logging;

namespace Herald.Events.Handlers;

public class ChannelEventHandler : IChannelEventHandler
{
    private readonly ILogger<ChannelEventHandler> _logger;
    
    public ChannelEventHandler(ILogger<ChannelEventHandler> logger)
    {
        _logger = logger;
    }

    public Task OnChannelCreated(DiscordClient client, ChannelCreateEventArgs args)
    {
        _logger.LogInformation("Channel Created Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnChannelUpdated(DiscordClient client, ChannelUpdateEventArgs args)
    {
        _logger.LogInformation("Channel Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnChannelDeleted(DiscordClient client, ChannelDeleteEventArgs args)
    {
        _logger.LogInformation("Channel Deleted Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnChannelPinsUpdated(DiscordClient client, ChannelPinsUpdateEventArgs args)
    {
        _logger.LogInformation("Channel Pins Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }
}