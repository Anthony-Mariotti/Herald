using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Herald.Bot.Events.Abstractions.Handlers;

public interface IScheduledGuildEventHandler
{
    public Task OnScheduledGuildEventCompleted(DiscordClient client, ScheduledGuildEventCompletedEventArgs args);
    public Task OnScheduledGuildEventCreated(DiscordClient client, ScheduledGuildEventCreateEventArgs args);
    public Task OnScheduledGuildEventDeleted(DiscordClient client, ScheduledGuildEventDeleteEventArgs args);
    public Task OnScheduledGuildEventUpdated(DiscordClient client, ScheduledGuildEventUpdateEventArgs args);
    public Task OnScheduledGuildEventUserAdded(DiscordClient client, ScheduledGuildEventUserAddEventArgs args);
    public Task OnScheduledGuildEventUserRemoved(DiscordClient client, ScheduledGuildEventUserRemoveEventArgs args);
}
