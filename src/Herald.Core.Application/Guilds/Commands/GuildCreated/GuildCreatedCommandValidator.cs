using FluentValidation;

namespace Herald.Core.Application.Guilds.Commands.GuildCreated;

public class GuildCreatedCommandValidator : AbstractValidator<GuildCreatedCommand>
{
    public GuildCreatedCommandValidator()
    {
        _ = RuleFor(v => v.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        _ = RuleFor(v => v.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");
    }
}