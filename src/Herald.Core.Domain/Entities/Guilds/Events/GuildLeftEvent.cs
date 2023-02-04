namespace Herald.Core.Domain.Entities.Guilds.Events;

internal class GuildLeftEvent : BaseEvent
{
    public ulong GuildId { get; }

    public GuildLeftEvent(ulong guildId)
    {
        GuildId = guildId;
    }
}