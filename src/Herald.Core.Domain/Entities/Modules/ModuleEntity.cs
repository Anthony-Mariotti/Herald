using MongoDB.Bson.Serialization.Attributes;

namespace Herald.Core.Domain.Entities.Modules;

public class ModuleEntity : BaseEntity
{
    [BsonRequired]
    public string? Name { get; set; }

    public ModuleEntity() { }

    public ModuleEntity(string name)
    {
        Name = name;
    }
    
    public static ModuleEntity Create(string name)
        => new ModuleEntity(name);
}