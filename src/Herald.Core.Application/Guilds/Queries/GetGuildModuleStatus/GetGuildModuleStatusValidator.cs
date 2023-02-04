using FluentValidation;
using Herald.Core.Domain.Entities.Modules;

namespace Herald.Core.Application.Guilds.Queries.GetGuildModuleStatus;

public class GetGuildModuleStatusValidator : AbstractValidator<GetGuildModuleStatusQuery>
{
    public GetGuildModuleStatusValidator()
    {
        _ = RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        _ = RuleFor(x => x.Module)
            .NotEmpty().WithMessage("ModuleName is required")
            .Must(BeAnAvailableModule).WithMessage("The specified module is not supported.");
    }
    
    private static bool BeAnAvailableModule(Module module) =>
        Module.AvailableModules.Contains(module);
}