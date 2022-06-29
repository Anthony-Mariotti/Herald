using FluentValidation;

namespace Herald.Core.Application.Soundtracks.Commands.PlayTrack;

public class PlayTrackCommandValidator : AbstractValidator<PlayTrack.PlayTrackCommand>
{
    public PlayTrackCommandValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        RuleFor(x => x.Track)
            .NotEmpty().WithMessage("Track is required.");
    }
}