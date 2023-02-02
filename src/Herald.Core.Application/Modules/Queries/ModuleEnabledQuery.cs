using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.ValueObjects.Modules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Modules.Queries;

public record ModuleEnabledQuery(ulong GuildId, HeraldModule Module) : IRequest<bool>;

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
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId));

        if (guild is null)
        {
            throw new NotFoundException(nameof(GuildEntity), request.GuildId);
        }

        return guild.Modules.Any(x => x.Name.Equals(request.Module.Name));
    }
}
