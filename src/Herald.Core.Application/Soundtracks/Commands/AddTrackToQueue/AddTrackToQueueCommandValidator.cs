using FluentValidation;

namespace Herald.Core.Application.Soundtracks.Commands.AddTrackToQueue;

public class AddTrackToQueueCommandValidator : AbstractValidator<AddTrackToQueueCommand>
{
    public AddTrackToQueueCommandValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        RuleFor(x => x.NotifyChannelId)
            .NotEmpty().WithMessage("NotifyChannelId is required.");

        RuleFor(x => x.Track)
            .NotEmpty().WithMessage("Track is required.");
    }
}