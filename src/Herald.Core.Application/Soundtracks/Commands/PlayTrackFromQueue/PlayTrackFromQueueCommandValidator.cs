using FluentValidation;

namespace Herald.Core.Application.Soundtracks.Commands.PlayTrackFromQueue;

public class PlayTrackFromQueueCommandValidator : AbstractValidator<PlayTrackFromQueueCommand>
{
    public PlayTrackFromQueueCommandValidator()
    {
        _ = RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage($"{nameof(PlayTrackFromQueueCommand)} requires a GuildId");
        
        _ = RuleFor(x => x.TrackIdentifier)
            .NotEmpty().WithMessage($"{nameof(PlayTrackFromQueueCommand)} requires a TrackIdentifier");
    }
}