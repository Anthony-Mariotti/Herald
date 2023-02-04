namespace Herald.Core.Domain.Entities.Modules;

public class ModuleAccess
{
    public ulong GuildId { get; private set; }

    public long ModuleId { get; private set; }

    public bool HasAccess { get; private set; } = false;

    public virtual Module Module { get; private set; } = default!;

    public ModuleAccess()
    {
    }

    public ModuleAccess(ulong guildId, long moduleId)
    {
        GuildId = guildId;
        ModuleId = moduleId;
        HasAccess = false;
    }

    public void Allow() =>
        HasAccess = true;

    public void Deny() =>
        HasAccess = false;
}
