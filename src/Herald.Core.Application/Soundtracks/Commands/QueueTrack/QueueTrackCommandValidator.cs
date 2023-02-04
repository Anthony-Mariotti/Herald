using FluentValidation;
using Herald.Core.Domain.Enums;
using Herald.Core.Domain.ValueObjects.Soundtracks;

namespace Herald.Core.Application.Soundtracks.Commands.QueueTrack;

public class QueueTrackCommandValidator : AbstractValidator<QueueTrackCommand>
{
    public QueueTrackCommandValidator()
    {
        _ = RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage($"{nameof(QueueTrackCommand)} requires a GuildId.");

        _ = RuleFor(x => x.Track)
            .NotEmpty().WithMessage($"{nameof(QueueTrackCommand)} requires a Track.")
            .Must(HaveQueuedStatus)
            .WithMessage($"{nameof(QueueTrackCommand)} requires a track status of \"{nameof(TrackStatus.Queued)}\".");
    }

    private static bool HaveQueuedStatus(QueuedTrackValue track)
        => track.Status.Equals(TrackStatus.Queued);
}