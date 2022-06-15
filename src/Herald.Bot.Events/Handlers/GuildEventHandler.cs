using DSharpPlus;
using DSharpPlus.EventArgs;
using Herald.Bot.Events.Abstractions.Handlers;
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
    
    public Task OnGuildCreated(DiscordClient client, GuildCreateEventArgs args)
    {
        _logger.LogInformation("Guild Created Event: {Guild}", args.Guild.Name);
        _mediator.Send(new GuildCreatedCommand
        {
            GuildId = args.Guild.Id,
            OwnerId = args.Guild.OwnerId
        });
        return Task.CompletedTask;
    }

    public Task OnGuildDeleted(DiscordClient client, GuildDeleteEventArgs args)
    {
        _logger.LogInformation("Guild Deleted Event: {Guild}", args.Guild.Name);
        _mediator.Send(new GuildDeletedCommand(args.Guild.Id));
        return Task.CompletedTask;
    }

    public Task OnGuildUpdated(DiscordClient client, GuildUpdateEventArgs args)
    {
        _logger.LogInformation("Guild Updated Event: {GuildAfter}", args.GuildAfter.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildAvailable(DiscordClient client, GuildCreateEventArgs args)
    {
        _logger.LogInformation("Guild Available Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildUnavailable(DiscordClient client, GuildDeleteEventArgs args)
    {
        _logger.LogInformation("Guild Unavailable Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildDownloadCompleted(DiscordClient client, GuildDownloadCompletedEventArgs args)
    {
        _logger.LogInformation("Guild Download Complete Event: {Guild}", string.Join(',', args.Guilds.Select(x => x.Value.Name)));
        return Task.CompletedTask;
    }

    public Task OnGuildEmojisUpdated(DiscordClient client, GuildEmojisUpdateEventArgs args)
    {
        _logger.LogInformation("Guild Emojis Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildStickersUpdated(DiscordClient client, GuildStickersUpdateEventArgs args)
    {
        _logger.LogInformation("Guild Stickers Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildIntegrationsUpdated(DiscordClient client, GuildIntegrationsUpdateEventArgs args)
    {
        _logger.LogInformation("Guild Integrations Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildBanAdded(DiscordClient client, GuildBanAddEventArgs args)
    {
        _logger.LogInformation("Guild Ban Added Event {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildBanRemoved(DiscordClient client, GuildBanRemoveEventArgs args)
    {
        _logger.LogInformation("Guild Ban Removed Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildMemberAdded(DiscordClient client, GuildMemberAddEventArgs args)
    {
        _logger.LogInformation("Guild Member Added Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildMemberRemoved(DiscordClient client, GuildMemberRemoveEventArgs args)
    {
        _logger.LogInformation("Guild Member Removed Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildMemberUpdated(DiscordClient client, GuildMemberUpdateEventArgs args)
    {
        _logger.LogInformation("Guild Member Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildMembersChunked(DiscordClient client, GuildMembersChunkEventArgs args)
    {
        _logger.LogInformation("Guild Members Chunked Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildRoleCreated(DiscordClient client, GuildRoleCreateEventArgs args)
    {
        _logger.LogInformation("Guild Role Created Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildRoleUpdated(DiscordClient client, GuildRoleUpdateEventArgs args)
    {
        _logger.LogInformation("Guild Role Updated Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }

    public Task OnGuildRoleDeleted(DiscordClient client, GuildRoleDeleteEventArgs args)
    {
        _logger.LogInformation("Guild Role Deleted Event: {Guild}", args.Guild.Name);
        return Task.CompletedTask;
    }
}