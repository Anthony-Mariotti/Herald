using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Modules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Guilds.Queries.GetGuildModuleStatus;

public record GetGuildModuleStatusQuery : IRequest<bool>
{
    public ulong GuildId { get; init; }

    public Module Module { get; init; } = default!;
}

public class GetGuildModuleStatusQueryHandler : IRequestHandler<GetGuildModuleStatusQuery, bool>
{
    private readonly IHeraldDbContext _context;

    public GetGuildModuleStatusQueryHandler(IHeraldDbContext context)
    {
        _context = context;
    }
    
    public async Task<bool> Handle(GetGuildModuleStatusQuery request, CancellationToken cancellationToken)
    {
        var guild = await _context.Guilds
            .Include(x => x.Modules)
            .SingleOrDefaultAsync(x => x.Id.Equals(request.GuildId), cancellationToken);

        return guild is null
            ? throw new NotFoundException(nameof(Guild), request.GuildId)
            : guild.HasAccess(request.Module);
    }
}