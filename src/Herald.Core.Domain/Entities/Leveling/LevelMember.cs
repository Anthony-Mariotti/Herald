namespace Herald.Core.Domain.Entities.Leveling;

public class LevelMember : BaseEntity
{
    public long LevelId { get; private set; }

    public ulong MemberId { get; private set; } // Discord Member Id

    public double Xp { get; private set; }

    public long Level { get; private set; }

    public LevelMember()
    {
    }

    public LevelMember(long levelId, ulong memberId, long xp, long level)
    {
        LevelId = levelId;
        MemberId = memberId;
        Xp = xp;
        Level = level;
    }

    public void AddXp(double xp) =>
        Xp += xp;
}
