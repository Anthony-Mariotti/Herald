namespace Herald.Core.Domain.Entities.Modules.Events;

public class ModuleReleasedEvent : BaseEvent
{
    public long ModuleId { get; }

    public ModuleReleasedEvent(long moduleId)
    {
        ModuleId = moduleId;
    }
}