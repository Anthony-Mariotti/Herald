using FluentValidation;

namespace Herald.Core.Application.Soundtracks.Commands.PlayNextTrack;

public class PlayNextTrackCommandValidator : AbstractValidator<PlayNextTrackCommand>
{
    public PlayNextTrackCommandValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        RuleFor(x => x.TrackIdentifier)
            .NotEmpty().WithMessage("TrackIdentifier is required.");
    }
}