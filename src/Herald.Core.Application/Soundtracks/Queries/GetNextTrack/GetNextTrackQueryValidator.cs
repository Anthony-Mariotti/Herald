using FluentValidation;

namespace Herald.Core.Application.Soundtracks.Queries.GetNextTrack;

public class GetNextTrackQueryValidator : AbstractValidator<GetNextTrackQuery>
{
    public GetNextTrackQueryValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");
    }
}