using Herald.Core.Domain.ValueObjects.Modules;

namespace Herald.Core.Domain.Entities.Guilds;

public class GuildEntity : BaseDomainEntity, IAggregateRoot
{
    // Primary Key
    public ulong GuildId { get; set; }

    public ulong OwnerId { get; set; }
    
    public bool Joined { get; set; }

    public DateTime JoinedOn { get; set; }

    public DateTime? LeftOn { get; set; }
    
    public ICollection<HeraldModule> Modules { get; set; } = new List<HeraldModule>();
    
    public GuildEntity() { }

    public GuildEntity(ulong guildId, ulong ownerId, DateTime? joinedOn, ICollection<HeraldModule>? modules)
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

    public static GuildEntity Create(ulong guildId, ulong ownerId, DateTime? joinedOn, ICollection<HeraldModule>? modules = null)
        => new(guildId, ownerId, joinedOn, modules);

    public void JoinedServer(ulong ownerId)
    {
        OwnerId = ownerId;
        Joined = true;
        LeftOn = null;
    }

    public void LeftServer(DateTime leftOn)
    {
        Joined = false;
        LeftOn = leftOn;
    }
    
    public bool EnableModule(HeraldModule requestedModule)
    {
        var module = Modules.SingleOrDefault(x => x.Equals(requestedModule));

        if (module is null)
        {
            Modules.Add(HeraldModule.Soundtrack);
            return true;
        }

        return false;
    }

    public bool DisableModule(HeraldModule requestedModule)
    {
        var module = Modules.SingleOrDefault(x => x.Equals(requestedModule));

        if (module is null)
            return false;
        
        Modules.Remove(module);
        return true;
    }
}