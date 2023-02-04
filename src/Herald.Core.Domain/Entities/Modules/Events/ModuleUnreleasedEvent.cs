namespace Herald.Core.Domain.Entities.Modules.Events;

public class ModuleUnreleasedEvent : BaseEvent
{
    public long ModuleId { get; }

    public ModuleUnreleasedEvent(long moduleId)
    {
        ModuleId = moduleId;
    }
}