using FluentValidation;
using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Modules;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Herald.Core.Application.Modules.Commands.ModuleEnable;

public class ModuleEnableCommandValidator : AbstractValidator<ModuleEnableCommand>
{
    private readonly IHeraldDbContext _context;

    public ModuleEnableCommandValidator(IHeraldDbContext context)
    {
        _context = context;

        RuleFor(x => x.GuildId)
            .NotEmpty().WithMessage("GuildId is required.");

        RuleFor(x => x.ModuleId)
            .NotEmpty().WithMessage("ModuleId is required.")
            .MustAsync(ModuleExist).WithMessage("The specified module does not exist.");
    }

    private async Task<bool> ModuleExist(ObjectId moduleId, CancellationToken cancellationToken)
    {
        var filter = Builders<ModuleEntity>.Filter
            .Where(x => x.Id.Equals(moduleId));

        return await _context.Modules.CountDocumentsAsync(filter, cancellationToken: cancellationToken) > 0;
    }
}