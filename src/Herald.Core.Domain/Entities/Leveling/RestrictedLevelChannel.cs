namespace Herald.Core.Domain.Entities.Leveling;

public class RestrictedLevelChannel : BaseEntity
{
    public long LevelId { get; private set; }

    public ulong ChannelId { get; private set; } // Discord Channel Id

    public RestrictedLevelChannel()
    {
    }

    public RestrictedLevelChannel(long levelId, ulong channelId)
    {
        LevelId = levelId;
        ChannelId = channelId;
    }
}