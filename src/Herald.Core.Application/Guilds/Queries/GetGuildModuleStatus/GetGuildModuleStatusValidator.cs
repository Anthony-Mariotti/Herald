using FluentValidation;

namespace Herald.Core.Application.Modules.Queries.GetModuleStatus;

public class GetGuildModuleStatusValidator : AbstractValidator<GetGuildModuleStatusQuery>
{
    public GetGuildModuleStatusValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        RuleFor(x => x.ModuleName)
            .NotEmpty().WithMessage("ModuleName is required");
    }
}