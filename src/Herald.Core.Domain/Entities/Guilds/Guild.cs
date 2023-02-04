using Herald.Core.Domain.Entities.Catalog;
using Herald.Core.Domain.Entities.Guilds.Events;
using Herald.Core.Domain.Entities.Members;
using Herald.Core.Domain.Entities.Modules;

namespace Herald.Core.Domain.Entities.Guilds;

public class Guild : BaseDomainEntity<ulong>, IAggregateRoot
{
    public ulong OwnerId { get; private set; }
    
    public bool Joined { get; private set; }

    public DateTime JoinedOn { get; private set; }

    public DateTime? LeftOn { get; private set; }
    
    private readonly List<ModuleAccess> _modules = new List<ModuleAccess>();
    public IReadOnlyCollection<ModuleAccess> Modules => _modules.AsReadOnly();

    private readonly List<CatalogItem> _items = new List<CatalogItem>();
    public IReadOnlyCollection<CatalogItem> Items => _items.AsReadOnly();

    private readonly List<Member> _members = new List<Member>();
    public IReadOnlyCollection<Member> Members => _members.AsReadOnly();
    
    public Guild() { }

    public Guild(ulong guildId, ulong ownerId, DateTime? joinedOn)
    {
        Id = guildId;
        OwnerId = ownerId;
        Joined = true;
        JoinedOn = joinedOn ?? DateTime.UtcNow;
    }

    public void JoinedServer(ulong ownerId)
    {
        Joined = true;

        if (LeftOn != null)
        {
            AddDomainEvent(new GuildJoinedEvent(Id));
        }
        else
        {
            AddDomainEvent(new GuildRejoinedEvent(Id));
        }

        LeftOn = null;
        SetOwner(ownerId);
    }

    public void LeftServer(DateTime leftOn)
    {
        Joined = false;
        LeftOn = leftOn;
        AddDomainEvent(new GuildLeftEvent(Id));
    }

    public void SetOwner(ulong ownerId)
    {
        OwnerId = ownerId;
        AddDomainEvent(new GuildOwnerChangedEvent(Id, ownerId));
    }

    public void AddMember(Member member)
    {
        member.GuildId = Id;
        _members.Add(member);

        AddDomainEvent(new GuildMemberAdded(Id, member.MemberId));
    }

    public void AddCatalogItem(CatalogItem item)
    {
        item.GuildId = Id;
        _items.Add(item);

        AddDomainEvent(new GuildCatalogItemAddedEvent(Id, item.Name, item.Price, item.Quantity));
    }

    public void EnableModule(Module module)
    {
        var access = _modules.SingleOrDefault(x => x.ModuleId == module.Id);

        if (access == null)
        {
            access = new ModuleAccess(Id, module.Id);
            _modules.Add(access);
        }

        access.Allow();
        AddDomainEvent(new GuildModuleEnabledEvent(Id, module.Id));
    }

    public void DisableModule(Module module)
    {
        var access = _modules.SingleOrDefault(x => x.ModuleId == module.Id);

        if (access == null)
        {
            access = new ModuleAccess(Id, module.Id);
            _modules.Add(access);
        }

        access.Deny();
        AddDomainEvent(new GuildModuleDisabledEvent(Id, module.Id));
    }

    public bool HasAccess(Module module) =>
        _modules.Any(x => x.ModuleId.Equals(module.Id) && x.HasAccess);
}