using FluentValidation;
using Herald.Core.Domain.ValueObjects.Modules;

namespace Herald.Core.Application.Modules.Commands.ModuleDisable;

public class ModuleDisableCommandValidator : AbstractValidator<ModuleDisableCommand>
{
    public ModuleDisableCommandValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        RuleFor(x => x.Module)
            .NotEmpty().WithMessage("ModuleName is required")
            .Must(BeAnAvailableModule).WithMessage("The specified module is not supported.");
    }
    
    private static bool BeAnAvailableModule(HeraldModule module) =>
        HeraldModule.HaveSupportFor(module);
}