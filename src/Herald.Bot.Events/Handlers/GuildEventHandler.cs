using DSharpPlus;
using DSharpPlus.EventArgs;
using Herald.Bot.Events.Abstractions.Handlers;
using Herald.Core.Application.Guilds.Commands.GuildAvailable;
using Herald.Core.Application.Guilds.Commands.GuildCreated;
using Herald.Core.Application.Guilds.Commands.GuildDeleted;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Events.Handlers;

public class GuildEventHandler : IGuildEventHandler
{
    private readonly ILogger<GuildEventHandler> _logger;
    private readonly ISender _mediator;

    public GuildEventHandler(ILogger<GuildEventHandler> logger, ISender mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }
    
    public async Task OnGuildCreated(DiscordClient client, GuildCreateEventArgs args)
    {
        _logger.LogDebug("Guild created event: {Guild}", args.Guild.Name);
        _ = await _mediator.Send(new GuildCreatedCommand
        {
            GuildId = args.Guild.Id,
            OwnerId = args.Guild.OwnerId
        });
    }

    public async Task OnGuildDeleted(DiscordClient client, GuildDeleteEventArgs args)
    {
        _logger.LogDebug("Guild deleted event: {Guild}", args.Guild.Name);
        _ = await _mediator.Send(new GuildDeletedCommand(args.Guild.Id));
    }

    public Task OnGuildUpdated(DiscordClient client, GuildUpdateEventArgs args)
    {
        _logger.LogDebug("Guild updated event: {GuildAfter}", args.GuildAfter.Name);
        return Task.CompletedTask;
    }

    public async Task OnGuildAvailable(DiscordClient client, GuildCreateEventArgs args)
    {
        _logger.LogDebug("Guild available event: {Guild}", args.Guild.Name);
        _ = await _mediator.Send(new GuildAvailableCommand(args.Guild.Id, args.Guild.OwnerId));
    }

    public Task OnGuildUnavailable(DiscordClient client, GuildDeleteEventArgs args)
    {
        _logger.LogDebug("Guild unavailable event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildDownloadCompleted(DiscordClient client, GuildDownloadCompletedEventArgs args)
    {
        _logger.LogDebug("Guild download complete event: {Guild}", string.Join(',', args.Guilds.Select(x => x.Value.Id)));
        return Task.CompletedTask;
    }

    public Task OnGuildEmojisUpdated(DiscordClient client, GuildEmojisUpdateEventArgs args)
    {
        _logger.LogDebug("Guild emojis updated event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildStickersUpdated(DiscordClient client, GuildStickersUpdateEventArgs args)
    {
        _logger.LogDebug("Guild stickers updated event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildIntegrationsUpdated(DiscordClient client, GuildIntegrationsUpdateEventArgs args)
    {
        _logger.LogDebug("Guild integrations updated event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildBanAdded(DiscordClient client, GuildBanAddEventArgs args)
    {
        _logger.LogDebug("Guild ban added event {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildBanRemoved(DiscordClient client, GuildBanRemoveEventArgs args)
    {
        _logger.LogDebug("Guild ban removed Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildMemberAdded(DiscordClient client, GuildMemberAddEventArgs args)
    {
        _logger.LogDebug("Guild member added Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildMemberRemoved(DiscordClient client, GuildMemberRemoveEventArgs args)
    {
        _logger.LogDebug("Guild member removed Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildMemberUpdated(DiscordClient client, GuildMemberUpdateEventArgs args)
    {
        _logger.LogDebug("Guild member updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildMembersChunked(DiscordClient client, GuildMembersChunkEventArgs args)
    {
        _logger.LogDebug("Guild members chunked Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildRoleCreated(DiscordClient client, GuildRoleCreateEventArgs args)
    {
        _logger.LogDebug("Guild role created Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildRoleUpdated(DiscordClient client, GuildRoleUpdateEventArgs args)
    {
        _logger.LogDebug("Guild role updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildRoleDeleted(DiscordClient client, GuildRoleDeleteEventArgs args)
    {
        _logger.LogDebug("Guild role deleted Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }
}