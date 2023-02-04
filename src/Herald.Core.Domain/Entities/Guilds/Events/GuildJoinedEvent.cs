namespace Herald.Core.Domain.Entities.Guilds.Events;

internal class GuildJoinedEvent : BaseEvent
{
    public ulong GuildId { get; }

    public GuildJoinedEvent(ulong guildId)
    {
        GuildId = guildId;
    }
}