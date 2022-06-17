namespace Herald.Core.Domain.Events.Guilds;

public class GuildAvailableEvent : BaseEvent
{
    public ulong GuildId { get; }
    
    public ulong OwnerId { get; }

    public GuildAvailableEvent(ulong guildId, ulong ownerId)
    {
        GuildId = guildId;
        OwnerId = ownerId;
    }
}