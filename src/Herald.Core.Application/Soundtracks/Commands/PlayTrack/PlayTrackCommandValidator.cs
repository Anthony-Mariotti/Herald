using FluentValidation;
using Herald.Core.Domain.Enums;
using Herald.Core.Domain.ValueObjects.Soundtracks;

namespace Herald.Core.Application.Soundtracks.Commands.PlayTrack;

public class PlayTrackCommandValidator : AbstractValidator<PlayTrackCommand>
{
    public PlayTrackCommandValidator()
    {
        _ = RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage($"{nameof(PlayTrackCommand)} required a GuildId.");

        _ = RuleFor(x => x.Track)
            .NotEmpty().WithMessage($"{nameof(PlayTrackCommand)} requires a Track.")
            .Must(HavePlayingStatus)
            .WithMessage($"{nameof(PlayTrackCommand)} requires a track status of \"{nameof(TrackStatus.Playing)}\".");
    }
    
    private static bool HavePlayingStatus(QueuedTrackValue track)
        => track.Status.Equals(TrackStatus.Playing);
}