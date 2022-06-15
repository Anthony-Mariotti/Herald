using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace Herald.Core.Domain.ValueObjects.Guilds;

public class GuildModule : ValueObject
{
    [BsonRequired]
    public MongoDBRef ModuleRef { get; set; } = default!;

    [BsonRequired]
    public bool Enabled { get; set; } = false;
    
    public GuildModule() { }

    public GuildModule(ObjectId moduleId, bool enabled)
    {
        ModuleRef = new MongoDBRef("Herald", "Modules", moduleId);
        Enabled = true;
    }

    public void Enable() => Enabled = true;

    public void Disable() => Enabled = false;

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Enabled;
    }
}