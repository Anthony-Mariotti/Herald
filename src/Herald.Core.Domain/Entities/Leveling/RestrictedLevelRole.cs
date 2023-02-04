namespace Herald.Core.Domain.Entities.Leveling;

public class RestrictedLevelRole : BaseEntity
{
    public long LevelId { get; private set; }

    public ulong RoleId { get; private set; } // Discord Role Id

    public RestrictedLevelRole()
    {
    }

    public RestrictedLevelRole(long levelId, ulong roleId)
    {
        LevelId = levelId;
        RoleId = roleId;
    }
}
