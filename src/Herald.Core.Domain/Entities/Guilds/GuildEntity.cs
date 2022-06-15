using Herald.Core.Domain.ValueObjects.Guilds;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Herald.Core.Domain.Entities.Guilds;

public class GuildEntity : BaseEntity, IAggregateRoot
{
    [BsonRequired]
    public ulong GuildId { get; set; }

    [BsonRequired]
    public ulong OwnerId { get; set; }
    
    public bool Joined { get; set; }

    public IList<GuildModule> Modules { get; set; } = new List<GuildModule>();

    public DateTime? JoinedOn { get; set; }

    [BsonIgnoreIfNull]
    public DateTime? LeftOn { get; set; }
    
    public GuildEntity() { }

    public GuildEntity(ulong guildId, ulong ownerId, DateTime? joinedOn, IList<GuildModule>? modules)
    {
        GuildId = guildId;
        OwnerId = ownerId;
        Joined = true;
        JoinedOn = joinedOn ?? DateTime.UtcNow;

        if (modules is not null)
        {
            Modules = modules;
        }
    }

    public static GuildEntity Create(ulong guildId, ulong ownerId, DateTime? joinedOn, IList<GuildModule>? modules = null)
        => new GuildEntity(guildId, ownerId, joinedOn, modules);

    public void EnableModule(ObjectId moduleId)
    {
        var module = Modules.SingleOrDefault(x => x.ModuleRef.Id.Equals(moduleId));

        if (module is not null)
        {
            module.Enable();
            return;
        }
        
        Modules.Add(new GuildModule(moduleId, true));
    }

    public void DisableModule(ObjectId moduleId)
    {
        var module = Modules.SingleOrDefault(x => x.ModuleRef.Id.Equals(moduleId));

        module?.Disable();
    }
}