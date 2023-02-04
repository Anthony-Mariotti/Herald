namespace Herald.Core.Domain.Entities.Guilds.Events;

public class GuildModuleEnabledEvent : BaseEvent
{
    public ulong GuildId { get; }
    public long ModuleId { get; }

    public GuildModuleEnabledEvent(ulong guildId, long moduleId)
    {
        GuildId = guildId;
        ModuleId = moduleId;
    }
}