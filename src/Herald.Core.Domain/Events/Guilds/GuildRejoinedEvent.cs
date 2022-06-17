namespace Herald.Core.Domain.Events.Guilds;

public class GuildRejoinedEvent : BaseEvent
{
    public ulong GuildId { get; }
    
    public ulong OwnerId { get; }

    public GuildRejoinedEvent(ulong guildId, ulong ownerId)
    {
        GuildId = guildId;
        OwnerId = ownerId;
    }
}