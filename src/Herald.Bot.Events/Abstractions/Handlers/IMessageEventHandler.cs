using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Herald.Bot.Events.Abstractions.Handlers;

public interface IMessageEventHandler
{
    public Task OnMessageCreated(DiscordClient client, MessageCreateEventArgs args);

    public Task OnMessageAcknowledged(DiscordClient client, MessageAcknowledgeEventArgs args);

    public Task OnMessageUpdated(DiscordClient client, MessageUpdateEventArgs args);

    public Task OnMessageDeleted(DiscordClient client, MessageDeleteEventArgs args);

    public Task OnMessagesBulkDeleted(DiscordClient client, MessageBulkDeleteEventArgs args);

    public Task OnMessageReactionAdded(DiscordClient client, MessageReactionAddEventArgs args);

    public Task OnMessageReactionRemoved(DiscordClient client, MessageReactionRemoveEventArgs args);

    public Task OnMessageReactionsCleared(DiscordClient client, MessageReactionsClearEventArgs args);

    public Task OnMessageReactionRemovedEmoji(DiscordClient client, MessageReactionRemoveEmojiEventArgs args);
}