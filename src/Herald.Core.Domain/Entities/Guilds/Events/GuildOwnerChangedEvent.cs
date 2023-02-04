namespace Herald.Core.Domain.Entities.Guilds.Events;

internal class GuildOwnerChangedEvent : BaseEvent
{
    public ulong Id { get; }
    public ulong OwnerId { get; }

    public GuildOwnerChangedEvent(ulong id, ulong ownerId)
    {
        Id = id;
        OwnerId = ownerId;
    }
}