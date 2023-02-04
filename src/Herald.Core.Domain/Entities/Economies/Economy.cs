namespace Herald.Core.Domain.Entities.Economies;

public class Economy : BaseDomainEntity
{
    public ulong GuildId { get; set; }

    public string Name { get; private set; } = "points";

    public bool ChannelRestriction { get; private set; }

    private readonly List<EconomyChannel> _channels = new List<EconomyChannel>();
    public IReadOnlyCollection<EconomyChannel> Channels => _channels.AsReadOnly();

    public Economy()
    {
    }

    public Economy(ulong guildId, string name, bool channelRestriction)
    {
        GuildId = guildId;
        Name = name;
        ChannelRestriction = channelRestriction;
    }
}
