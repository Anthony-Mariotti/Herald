namespace Herald.Core.Domain.Entities.Guilds.Events;

internal class GuildRejoinedEvent : BaseEvent
{
    public ulong GuildId { get; }

    public GuildRejoinedEvent(ulong guildId)
    {
        GuildId = guildId;
    }
}