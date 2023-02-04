namespace Herald.Core.Domain.Entities.Guilds.Events;

internal class GuildModuleDisabledEvent : BaseEvent
{
    public ulong GuildId { get; }
    public long ModuleId { get; }

    public GuildModuleDisabledEvent(ulong guildId, long moduleId)
    {
        GuildId = guildId;
        ModuleId = moduleId;
    }
}