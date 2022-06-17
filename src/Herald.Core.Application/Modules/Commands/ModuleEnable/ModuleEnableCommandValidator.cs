using FluentValidation;
using Herald.Core.Domain.ValueObjects.Modules;

namespace Herald.Core.Application.Modules.Commands.ModuleEnable;

public class ModuleEnableCommandValidator : AbstractValidator<ModuleEnableCommand>
{
    public ModuleEnableCommandValidator()
    {
        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        RuleFor(x => x.Module)
            .NotEmpty().WithMessage("Module is required.")
            .Must(BeAnAvailableModule).WithMessage("The specified module is not supported.");
    }

    private static bool BeAnAvailableModule(HeraldModule module) =>
        HeraldModule.HaveSupportFor(module);
}