using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Modules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Modules.Commands.ModuleDisable;

public record ModuleDisableCommand : IRequest
{
    public ulong GuildId { get; init; }

    public Module Module { get; init; } = default!;
}

public class ModuleDisableCommandHandler : IRequestHandler<ModuleDisableCommand>
{
    private readonly IHeraldDbContext _context;

    public ModuleDisableCommandHandler(IHeraldDbContext context)
    {
        _context = context;
    }
    
    public async Task<Unit> Handle(ModuleDisableCommand request, CancellationToken cancellationToken)
    {
        var guild = await _context.Guilds
            .Include(x => x.Modules)
            .SingleOrDefaultAsync(x => x.Id.Equals(request.GuildId), cancellationToken);

        if (guild is null)
        {
            throw new NotFoundException(nameof(Guild), request.GuildId);
        }

        guild.DisableModule(request.Module);

        _ = await _context.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}