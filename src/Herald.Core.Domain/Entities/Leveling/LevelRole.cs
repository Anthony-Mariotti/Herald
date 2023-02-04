namespace Herald.Core.Domain.Entities.Leveling;

public class LevelRole : BaseEntity
{
    public long LevelId { get; private set; }

    public ulong RoleId { get; private set; } // Discord RoleId

    public long AtLevel { get; private set; }

    public LevelRole()
    {
    }

    public LevelRole(long levelId, ulong roleId, long atLevel)
    {
        LevelId = levelId;
        RoleId = roleId;
        AtLevel = atLevel;
    }
}
