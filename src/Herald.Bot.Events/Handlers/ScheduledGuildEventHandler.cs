using DSharpPlus;
using DSharpPlus.EventArgs;
using Herald.Bot.Events.Abstractions.Handlers;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Events.Handlers;

public class ScheduledGuildEventHandler : IScheduledGuildEventHandler
{
    private readonly ILogger<ScheduledGuildEventHandler> _logger;

    public ScheduledGuildEventHandler(ILogger<ScheduledGuildEventHandler> logger)
    {
        _logger = logger;
    }

    public Task OnScheduledGuildEventCompleted(DiscordClient client, ScheduledGuildEventCompletedEventArgs args)
    {
        _logger.LogDebug("Scheduled guild event {Event} completed: {Guild}", args.Event.Id, args.Event.GuildId);
        return Task.CompletedTask;
    }

    public Task OnScheduledGuildEventCreated(DiscordClient client, ScheduledGuildEventCreateEventArgs args)
    {
        _logger.LogDebug("Scheduled guild event {Event} created: {Guild}", args.Event.Id, args.Event.GuildId);
        return Task.CompletedTask;
    }

    public Task OnScheduledGuildEventDeleted(DiscordClient client, ScheduledGuildEventDeleteEventArgs args)
    {
        _logger.LogDebug("Scheduled guild event {Event} deleted: {Guild}", args.Event.Id, args.Event.GuildId);
        return Task.CompletedTask;
    }

    public Task OnScheduledGuildEventUpdated(DiscordClient client, ScheduledGuildEventUpdateEventArgs args)
    {
        _logger.LogDebug("Scheduled guild event {Event} updated: {Guild}", args.EventAfter.Id, args.EventAfter.GuildId);
        return Task.CompletedTask;
    }

    public Task OnScheduledGuildEventUserAdded(DiscordClient client, ScheduledGuildEventUserAddEventArgs args)
    {
        _logger.LogDebug("Scheduled guild event {Event} user added: {Guild}", args.Event.Id, args.Event.GuildId);
        return Task.CompletedTask;
    }

    public Task OnScheduledGuildEventUserRemoved(DiscordClient client, ScheduledGuildEventUserRemoveEventArgs args)
    {
        _logger.LogDebug("Scheduled guild event {Event} user removed: {Guild}", args.Event.Id, args.Event.GuildId);
        return Task.CompletedTask;
    }
}
