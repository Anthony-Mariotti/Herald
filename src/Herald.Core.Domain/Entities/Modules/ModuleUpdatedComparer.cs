namespace Herald.Core.Domain.Entities.Modules;

public class ModuleUpdatedComparer : IEqualityComparer<Module>
{
    public bool Equals(Module? x, Module? y) =>
        x?.Id == y?.Id && x?.Name == y?.Name;

    public int GetHashCode(Module obj) =>
        obj.Id.GetHashCode();
}
