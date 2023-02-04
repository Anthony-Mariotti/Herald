namespace Herald.Core.Domain.Entities.Economies;

public class EconomyChannel
{
    public long EconomyId { get; private set; }

    public ulong ChannelId { get; private set; }

    public EconomyChannel()
    {
    }

    public EconomyChannel(long economyId, ulong channelId)
    {
        EconomyId = economyId;
        ChannelId = channelId;
    }
}