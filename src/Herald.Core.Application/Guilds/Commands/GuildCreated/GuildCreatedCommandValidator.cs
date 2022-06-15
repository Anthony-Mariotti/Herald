using FluentValidation;
using Herald.Core.Application.Abstractions;

namespace Herald.Core.Application.Guilds.Commands.GuildCreated;

public class GuildCreatedCommandValidator : AbstractValidator<GuildCreatedCommand>
{
    public GuildCreatedCommandValidator(IHeraldDbContext context)
    {
        RuleFor(v => v.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        RuleFor(v => v.OwnerId)
            .NotEmpty().WithMessage("OwnerId is required.");
    }
}