using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Modules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Modules.Queries;

public record ModuleEnabledQuery(ulong GuildId, Module Module) : IRequest<bool>;

public class ModuleEnabledQueryHandler : IRequestHandler<ModuleEnabledQuery, bool>
{
    private readonly IHeraldDbContext _context;

    public ModuleEnabledQueryHandler(IHeraldDbContext context)
    {
        _context = context;
    }

    public async Task<bool> Handle(ModuleEnabledQuery request, CancellationToken cancellationToken)
    {
        var guild = await _context.Guilds
            .Include(x => x.Modules)
            .SingleOrDefaultAsync(x => x.Id.Equals(request.GuildId), cancellationToken);

        return guild is null
            ? throw new NotFoundException(nameof(Guild), request.GuildId)
            : guild.HasAccess(request.Module);
    }
}
