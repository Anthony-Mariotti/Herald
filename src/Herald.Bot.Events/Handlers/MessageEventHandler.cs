using DSharpPlus;
using DSharpPlus.EventArgs;
using Herald.Bot.Events.Abstractions.Handlers;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Events.Handlers;

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
        
        _logger.LogDebug("Message Created Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageAcknowledged(DiscordClient client, MessageAcknowledgeEventArgs args)
    {
        _logger.LogDebug("Message Acknowledged Event: {Guild}", args.Channel.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageUpdated(DiscordClient client, MessageUpdateEventArgs args)
    {
        if (args.Author.IsBot) return Task.CompletedTask;
        
        _logger.LogDebug("Message Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageDeleted(DiscordClient client, MessageDeleteEventArgs args)
    {
        _logger.LogDebug("Message Deleted Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessagesBulkDeleted(DiscordClient client, MessageBulkDeleteEventArgs args)
    {
        _logger.LogDebug("Messages Bulk Deleted Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionAdded(DiscordClient client, MessageReactionAddEventArgs args)
    {
        _logger.LogDebug("Message Reaction Added Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionRemoved(DiscordClient client, MessageReactionRemoveEventArgs args)
    {
        _logger.LogDebug("Message Reaction Removed Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionsCleared(DiscordClient client, MessageReactionsClearEventArgs args)
    {
        _logger.LogDebug("Message Reactions Cleared Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionRemovedEmoji(DiscordClient client, MessageReactionRemoveEmojiEventArgs args)
    {
        _logger.LogDebug("Message Reaction Removed Emoji Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }
}