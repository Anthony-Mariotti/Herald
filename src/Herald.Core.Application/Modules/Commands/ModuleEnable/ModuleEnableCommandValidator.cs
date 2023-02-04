using FluentValidation;
using Herald.Core.Domain.Entities.Modules;

namespace Herald.Core.Application.Modules.Commands.ModuleEnable;

public class ModuleEnableCommandValidator : AbstractValidator<ModuleEnableCommand>
{
    public ModuleEnableCommandValidator()
    {
        _ = RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        _ = RuleFor(x => x.Module)
            .NotEmpty().WithMessage("Module is required.")
            .Must(BeAnAvailableModule).WithMessage("The specified module is not supported.");
    }

    private static bool BeAnAvailableModule(Module module) =>
        Module.AvailableModules.Contains(module);
}