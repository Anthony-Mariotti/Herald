using FluentValidation;

namespace Herald.Core.Application.Soundtracks.Commands.PlayTrackCommand;

public class PlayTrackCommandValidator : AbstractValidator<PlayTrackCommand>
{
    public PlayTrackCommandValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        RuleFor(x => x.NotifyChannelId)
            .NotEmpty().WithMessage("NotifyChannelId is required.");

        RuleFor(x => x.RequestUserId)
            .NotEmpty().WithMessage("RequestUserId is required.");

        RuleFor(x => x.Track)
            .NotEmpty().WithMessage("Track is required.");
    }
}