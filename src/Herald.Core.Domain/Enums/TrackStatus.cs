using Ardalis.SmartEnum;

namespace Herald.Core.Domain.Enums;

public sealed class TrackStatus : SmartEnum<TrackStatus>
{
    public static readonly TrackStatus Queued = new(nameof(Queued), 1);

    public static readonly TrackStatus Playing = new(nameof(Playing), 2);

    public static readonly TrackStatus Paused = new(nameof(Paused), 3);

    public static readonly TrackStatus Played = new(nameof(Played), 4);

    public static readonly TrackStatus Skipped = new(nameof(Skipped), 5);
    
    public static readonly TrackStatus Failed = new(nameof(Failed), 6);

    private TrackStatus(string name, int value) : base(name, value) { }
}