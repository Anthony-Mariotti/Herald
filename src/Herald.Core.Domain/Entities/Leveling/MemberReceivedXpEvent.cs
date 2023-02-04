namespace Herald.Core.Domain.Entities.Leveling;

internal class MemberReceivedXpEvent : BaseEvent
{
    public ulong MemberId { get; }
    public double PreviousXp { get; }
    public double AwardedXp { get; }
    public double CurrentXp { get; private set; }

    public MemberReceivedXpEvent(ulong memberId, double previousXp, double awardedXp)
    {
        MemberId = memberId;
        PreviousXp = previousXp;
        AwardedXp = awardedXp;
    }

    public void SetCurrentXp(double currentXp) => CurrentXp = currentXp;
}