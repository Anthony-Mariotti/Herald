namespace Herald.Core.Domain.Entities.Leveling;

public class Level : BaseDomainEntity
{
    public ulong GuildId { get; set; }

    public bool ChanncelRestriction { get; private set; }

    public bool RoleRestriction { get; private set; }

    public bool RoleMode { get; private set; }

    public double XpRate { get; private set; }

    private readonly List<LevelMember> _members = new List<LevelMember>();
    public IReadOnlyCollection<LevelMember> Members => _members.AsReadOnly();

    private readonly List<LevelRole> _roles = new List<LevelRole>();
    public IReadOnlyCollection<LevelRole> Roles => _roles.AsReadOnly();

    private readonly List<RestrictedLevelRole> _restrictedRoles = new List<RestrictedLevelRole>();
    public IReadOnlyCollection<RestrictedLevelRole> RestrictedRoles => _restrictedRoles.AsReadOnly();

    private readonly List<RestrictedLevelChannel> _restrictedChannels = new List<RestrictedLevelChannel>();
    public IReadOnlyCollection<RestrictedLevelChannel> ResitrctedChannels => _restrictedChannels.AsReadOnly();

    public Level()
    {
    }

    public Level(ulong guildId, bool channcelRestriction, bool roleRestriction, bool roleMode, double xpRate, List<LevelMember> members, List<LevelRole> roles, List<RestrictedLevelRole> restrictedRoles, List<RestrictedLevelChannel> restrictedChannels)
    {
        GuildId = guildId;
        ChanncelRestriction = channcelRestriction;
        RoleRestriction = roleRestriction;
        RoleMode = roleMode;
        XpRate = xpRate;
        _members = members ?? throw new ArgumentNullException(nameof(members));
        _roles = roles ?? throw new ArgumentNullException(nameof(roles));
        _restrictedRoles = restrictedRoles ?? throw new ArgumentNullException(nameof(restrictedRoles));
        _restrictedChannels = restrictedChannels ?? throw new ArgumentNullException(nameof(restrictedChannels));
    }

    public void RewardXp(ulong memberId)
    {
        var member = _members.SingleOrDefault(x => x.MemberId.Equals(memberId));

        if (member is null)
        {
            member = new LevelMember(Id, memberId, 0, 0);
            _members.Add(member);
        }

        var xp = 1 * XpRate;

        var domainEvent = new MemberReceivedXpEvent(memberId, member.Xp, xp);
        member.AddXp(xp);

        domainEvent.SetCurrentXp(member.Xp);
        AddDomainEvent(domainEvent);
    }
}
