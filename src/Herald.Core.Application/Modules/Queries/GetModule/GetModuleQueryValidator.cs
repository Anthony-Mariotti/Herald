using FluentValidation;

namespace Herald.Core.Application.Modules.Queries.GetModule;

public class GetModuleQueryValidator : AbstractValidator<GetModuleQuery>
{
    public GetModuleQueryValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty().WithMessage("Name is required.");
    }
}