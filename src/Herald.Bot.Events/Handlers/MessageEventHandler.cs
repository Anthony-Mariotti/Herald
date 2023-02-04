using DSharpPlus;
using DSharpPlus.EventArgs;
using Herald.Bot.Events.Abstractions.Handlers;
using Herald.Core.Application.Events.Notification.MessageCreated;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Events.Handlers;

public class MessageEventHandler : IMessageEventHandler
{
    private readonly ILogger<MessageEventHandler> _logger;
    private readonly IPublisher _publisher;

    public MessageEventHandler(ILogger<MessageEventHandler> logger, IPublisher publisher)
    {
        _logger = logger;
        _publisher = publisher;
    }
    
    public async Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs args)
    {
        if (args.Author.IsBot)
        {
            return;
        }

        _logger.LogDebug("Message created event: {Message} in {Guild} by {User}", args.Message.Id, args.Guild.Id, args.Author.Id);

        if (args.Channel.IsPrivate)
        {
            return;
        }

        await _publisher.Publish(new MessageCreatedNotification(args));
    }

    public Task OnMessageAcknowledged(DiscordClient client, MessageAcknowledgeEventArgs args)
    {
        _logger.LogDebug("Message acknowledged event: {Guild}", args.Channel.Guild.Id);
        return Task.CompletedTask;
    }

    public Task OnMessageUpdated(DiscordClient client, MessageUpdateEventArgs args)
    {
        if (args.Author.IsBot)
        {
            return Task.CompletedTask;
        }

        _logger.LogDebug("Message updated event: {Message} in {Guild} by {User}", args.Message.Id, args.Guild.Id, args.Author.Id);
        return Task.CompletedTask;
    }

    public Task OnMessageDeleted(DiscordClient client, MessageDeleteEventArgs args)
    {
        _logger.LogDebug("Message deleted event: {Message} in {Guild} by {User}", args.Message.Id, args.Guild.Id, args.Message.Author.Id);
        return Task.CompletedTask;
    }

    public Task OnMessagesBulkDeleted(DiscordClient client, MessageBulkDeleteEventArgs args)
    {
        _logger.LogDebug("Messages bulk deleted event: {@Message} in {Guild}", args.Messages, args.Guild.Id);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionAdded(DiscordClient client, MessageReactionAddEventArgs args)
    {
        _logger.LogDebug("Message reaction added event: {Message} in {Guild} by {User}", args.Message.Id, args.Guild.Id, args.User.Id);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionRemoved(DiscordClient client, MessageReactionRemoveEventArgs args)
    {
        _logger.LogDebug("Message reaction removed event: {Message} in {Guild} by {User}", args.Message.Id, args.Guild.Id, args.User.Id);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionsCleared(DiscordClient client, MessageReactionsClearEventArgs args)
    {
        _logger.LogDebug("Message reactions cleared event: {Message} in {Guild} by {User}", args.Message.Id, args.Guild.Id, args.Message.Author.Id);
        return Task.CompletedTask;
    }

    public Task OnMessageReactionRemovedEmoji(DiscordClient client, MessageReactionRemoveEmojiEventArgs args)
    {
        _logger.LogDebug("Message reaction removed emoji event: {Emoji} on {Message} in {Guild} by {User}", args.Emoji.Id, args.Message.Id, args.Guild.Id, args.Message.Author.Id);
        return Task.CompletedTask;
    }
}