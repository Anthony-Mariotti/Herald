namespace Herald.Core.Domain.Entities.Guilds.Events;

internal class GuildMemberAdded : BaseEvent
{
    public ulong GuildId { get; }
    public ulong MemberId { get; }

    public GuildMemberAdded(ulong guildId, ulong memberId)
    {
        GuildId = guildId;
        MemberId = memberId;
    }
}