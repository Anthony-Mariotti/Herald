using Ardalis.SmartEnum;

namespace Herald.Core.Domain.Enums;

public sealed class TrackStatusReason : SmartEnum<TrackStatusReason>
{
    public static readonly TrackStatusReason TrackEnded = new(nameof(TrackEnded), 1);

    public static readonly TrackStatusReason UserStopped = new(nameof(UserStopped), 2);

    public static readonly TrackStatusReason UserPaused = new(nameof(UserPaused), 3);

    public static readonly TrackStatusReason UserSkipped = new(nameof(UserSkipped), 4);

    public static readonly TrackStatusReason TrackFailed = new(nameof(TrackFailed), 5);

    public static readonly TrackStatusReason TrackCleanUp = new(nameof(TrackCleanUp), 6);
    
    private TrackStatusReason(string name, int value) : base(name, value) { }
}