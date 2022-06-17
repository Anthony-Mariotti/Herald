namespace Herald.Core.Domain.Events.Guilds;

public class GuildCreatedEvent : BaseEvent
{
    public ulong GuildId { get; }
    
    public ulong OwnerId { get; }

    public GuildCreatedEvent(ulong guildId, ulong ownerId)
    {
        GuildId = guildId;
        OwnerId = ownerId;
    }
}