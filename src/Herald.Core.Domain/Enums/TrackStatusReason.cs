using Ardalis.SmartEnum;

namespace Herald.Core.Domain.Enums;

public sealed class TrackStatusReason : SmartEnum<TrackStatusReason>
{
    public static readonly TrackStatusReason FromQueue = new(nameof(FromQueue), 1);
    
    public static readonly TrackStatusReason TrackEnded = new(nameof(TrackEnded), 2);

    public static readonly TrackStatusReason UserAdded = new(nameof(UserAdded), 3);

    public static readonly TrackStatusReason UserStopped = new(nameof(UserStopped), 4);

    public static readonly TrackStatusReason UserPaused = new(nameof(UserPaused), 5);

    public static readonly TrackStatusReason UserResumed = new(nameof(UserResumed), 6);

    public static readonly TrackStatusReason UserSkipped = new(nameof(UserSkipped), 7);

    public static readonly TrackStatusReason TrackFailed = new(nameof(TrackFailed), 8);

    public static readonly TrackStatusReason TrackCleanUp = new(nameof(TrackCleanUp), 9);
    
    private TrackStatusReason(string name, int value) : base(name, value) { }
}