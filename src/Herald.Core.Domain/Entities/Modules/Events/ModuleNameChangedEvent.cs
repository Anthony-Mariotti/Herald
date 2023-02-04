namespace Herald.Core.Domain.Entities.Modules.Events;

internal class ModuleNameChangedEvent : BaseEvent
{
    public long ModuleId { get; }
    public string PreviousName { get; }
    public string CurrentName { get; }

    public ModuleNameChangedEvent(long moduleId, string previousName, string currentName)
    {
        ModuleId = moduleId;
        PreviousName = previousName;
        CurrentName = currentName;
    }
}