using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.ValueObjects.Modules;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Herald.Core.Application.Guilds.Queries.GetGuildModuleStatus;

public record GetGuildModuleStatusQuery : IRequest<bool>
{
    public ulong GuildId { get; init; }

    public HeraldModule Module { get; init; } = default!;
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
        // var filter = Builders<GuildEntity>.Filter
        //     .Eq(x => x.GuildId, request.GuildId);

        var guild = await _context.Guilds
            .Include(x => x.Modules)
            .SingleOrDefaultAsync(x => x.GuildId.Equals(request.GuildId), cancellationToken);

        if (guild is null)
            throw new NotFoundException(nameof(GuildEntity), request.GuildId);

        return guild.Modules.Contains(request.Module);
    }
}