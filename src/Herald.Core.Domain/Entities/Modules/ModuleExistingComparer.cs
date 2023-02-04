namespace Herald.Core.Domain.Entities.Modules;

public class ModuleExistingComparer : IEqualityComparer<Module>
{
    public bool Equals(Module? x, Module? y) =>
        x?.Id == y?.Id;

    public int GetHashCode(Module obj) =>
        obj.Id.GetHashCode();
}
