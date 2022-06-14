using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Herald.Events.Abstractions.Handlers;

public interface IGuildEventHandler
{
    public Task OnGuildCreated(DiscordClient client, GuildCreateEventArgs args);

    public Task OnGuildDeleted(DiscordClient client, GuildDeleteEventArgs args);

    public Task OnGuildUpdated(DiscordClient client, GuildUpdateEventArgs args);

    public Task OnGuildAvailable(DiscordClient client, GuildCreateEventArgs args);

    public Task OnGuildUnavailable(DiscordClient client, GuildDeleteEventArgs args);

    public Task OnGuildDownloadCompleted(DiscordClient client, GuildDownloadCompletedEventArgs args);

    public Task OnGuildEmojisUpdated(DiscordClient client, GuildEmojisUpdateEventArgs args);

    public Task OnGuildStickersUpdated(DiscordClient client, GuildStickersUpdateEventArgs args);

    public Task OnGuildIntegrationsUpdated(DiscordClient client, GuildIntegrationsUpdateEventArgs args);

    public Task OnGuildBanAdded(DiscordClient client, GuildBanAddEventArgs args);

    public Task OnGuildBanRemoved(DiscordClient client, GuildBanRemoveEventArgs args);

    public Task OnGuildMemberAdded(DiscordClient client, GuildMemberAddEventArgs args);

    public Task OnGuildMemberRemoved(DiscordClient client, GuildMemberRemoveEventArgs args);

    public Task OnGuildMemberUpdated(DiscordClient client, GuildMemberUpdateEventArgs args);

    public Task OnGuildMembersChunked(DiscordClient client, GuildMembersChunkEventArgs args);

    public Task OnGuildRoleCreated(DiscordClient client, GuildRoleCreateEventArgs args);

    public Task OnGuildRoleUpdated(DiscordClient client, GuildRoleUpdateEventArgs args);

    public Task OnGuildRoleDeleted(DiscordClient client, GuildRoleDeleteEventArgs args);
}