namespace Herald.Core.Domain.ValueObjects.Modules;

public class HeraldModule : ValueObject
{
    public string Name { get; private set; }
    
    private HeraldModule()
    {
        Name = string.Empty;
    }

    private HeraldModule(string name)
    {
        Name = name;
    }

    public static HeraldModule Soundtrack => new(nameof(Soundtrack));

    public static HeraldModule Reward => new(nameof(Reward));

    public static HeraldModule From(string name)
    {
        var module = new HeraldModule(name);

        if (!Supported.Contains(module))
            throw new NotSupportedException(name);

        return module;
    }

    public static bool HaveSupportFor(HeraldModule module)
        => Supported.Contains(module);
    
    public static implicit operator string(HeraldModule module) => module.ToString();
    
    public static explicit operator HeraldModule(string name) => From(name);
    
    public override string ToString() => Name;

    protected static IEnumerable<HeraldModule> Supported
    {
        get
        {
            yield return Soundtrack;
        }
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Name;
    }
}