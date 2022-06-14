﻿using DSharpPlus;
using DSharpPlus.EventArgs;
using Herald.Events.Abstractions.Handlers;
using Microsoft.Extensions.Logging;

namespace Herald.Events.Handlers;

public class MessageEventHandler : IMessageEventHandler
{
    private readonly ILogger<MessageEventHandler> _logger;

    public MessageEventHandler(ILogger<MessageEventHandler> logger)
    {
        _logger = logger;
    }
    
    public Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs args)
    {
        if (args.Author.IsBot) return Task.CompletedTask;
        
        _logger.LogInformation("Message Created Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageAcknowledged(DiscordClient client, MessageAcknowledgeEventArgs args)
    {
        _logger.LogInformation("Message Acknowledged Event: {Guild}", args.Channel.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageUpdated(DiscordClient client, MessageUpdateEventArgs args)
    {
        if (args.Author.IsBot) return Task.CompletedTask;
        
        _logger.LogInformation("Message Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageDeleted(DiscordClient client, MessageDeleteEventArgs args)
    {
        _logger.LogInformation("Message Deleted Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessagesBulkDeleted(DiscordClient client, MessageBulkDeleteEventArgs args)
    {
        _logger.LogInformation("Messages Bulk Deleted Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionAdded(DiscordClient client, MessageReactionAddEventArgs args)
    {
        _logger.LogInformation("Message Reaction Added Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionRemoved(DiscordClient client, MessageReactionRemoveEventArgs args)
    {
        _logger.LogInformation("Message Reaction Removed Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionsCleared(DiscordClient client, MessageReactionsClearEventArgs args)
    {
        _logger.LogInformation("Message Reactions Cleared Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionRemovedEmoji(DiscordClient client, MessageReactionRemoveEmojiEventArgs args)
    {
        _logger.LogInformation("Message Reaction Removed Emoji Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }
}