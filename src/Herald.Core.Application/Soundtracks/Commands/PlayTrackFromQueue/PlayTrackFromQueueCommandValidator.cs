using FluentValidation;

namespace Herald.Core.Application.Soundtracks.Commands.PlayTrackFromQueue;

public class PlayTrackFromQueueCommandValidator : AbstractValidator<PlayTrackFromQueueCommand>
{
    public PlayTrackFromQueueCommandValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage($"{nameof(PlayTrackFromQueueCommand)} requires a GuildId");
        
        RuleFor(x => x.TrackIdentifier)
            .NotEmpty().WithMessage($"{nameof(PlayTrackFromQueueCommand)} requires a TrackIdentifier");
    }
}