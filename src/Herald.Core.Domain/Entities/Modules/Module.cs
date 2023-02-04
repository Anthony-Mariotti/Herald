using Herald.Core.Domain.Entities.Modules.Events;
using System.Diagnostics.CodeAnalysis;

namespace Herald.Core.Domain.Entities.Modules;

public class Module : BaseDomainEntity, IAggregateRoot
{
    public string Name { get; private set; }

	public bool Released { get; private set; }

	public Module()
	{
        Name = string.Empty;
	}

	public Module(string name, bool released)
    {
        Name = name;
        Released = released;
    }

    public void Release()
    {
        Released = true;
        AddDomainEvent(new ModuleReleasedEvent(Id));
    }

    public void UnRelease()
    {
        Released = false;
        AddDomainEvent(new ModuleUnreleasedEvent(Id));
    }

    public void SetName(string name)
    {
        AddDomainEvent(new ModuleNameChangedEvent(Id, Name, name));
        Name = name;
    }

    [SuppressMessage("Style", "IDE0046:Convert to conditional expression", Justification = "Readability")]
    public static Module From(string name)
    {
        var module = AvailableModules.SingleOrDefault(x => x.Name.Equals(name));

        if (module == null)
        {
            return new Module
            {
                Id = -1,
                Name = "Invalid Module",
                Released = false
            };
        }

        return module;
    }


#pragma warning disable CA2211 // Use for static visibility outside of this root
    public static Module Soundtrack = new Module
    {
        Id = 1,
        Name = "Soundtrack",
        Released = true
    };

    public static Module Economy = new Module
    {
        Id = 2,
        Name = "Economy",
        Released = true
    };

    public static Module AnyDeal = new Module
    {
        Id = 3,
        Name = "AnyDeal",
        Released = false
    };
#pragma warning restore CA2211 // Non-constant fields should not be visible


    public static IReadOnlyCollection<Module> AvailableModules => 
        new List<Module>
            {
                Soundtrack,
                Economy,
                AnyDeal
            }.AsReadOnly();

}