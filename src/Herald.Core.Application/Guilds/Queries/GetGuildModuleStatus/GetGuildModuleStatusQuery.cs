using AutoMapper;
using Herald.Core.Application.Abstractions;
using Herald.Core.Application.Exceptions;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Modules;
using MediatR;
using MongoDB.Driver;

namespace Herald.Core.Application.Modules.Queries.GetModuleStatus;

public record GetGuildModuleStatusQuery : IRequest<GuildModuleStatus>
{
    public ulong GuildId { get; init; }
    
    public string? ModuleName { get; init; }
}

public class GetGuildModuleStatusQueryHandler : IRequestHandler<GetGuildModuleStatusQuery, GuildModuleStatus>
{
    private readonly IHeraldDbContext _context;
    private readonly IMapper _mapper;

    public GetGuildModuleStatusQueryHandler(IHeraldDbContext context, IMapper _mapper)
    {
        _context = context;
        this._mapper = _mapper;
    }
    
    public async Task<GuildModuleStatus> Handle(GetGuildModuleStatusQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<GuildEntity>.Filter
            .Eq(x => x.GuildId, request.GuildId);

        var guild = await _context.Guilds.Find(filter)
            .Limit(1)
            .SingleOrDefaultAsync(cancellationToken);

        if (guild is null)
        {
            throw new NotFoundException(nameof(GuildEntity), request.GuildId);
        }

        var moduleFilter = Builders<ModuleEntity>.Filter
            .Eq(x => x.Name, request.ModuleName);

        var module = await _context.Modules.Find(moduleFilter)
            .Limit(1)
            .SingleOrDefaultAsync(cancellationToken);

        if (module is null)
        {
            throw new NotFoundException(nameof(ModuleEntity), request.ModuleName!);
        }

        return _mapper.Map<GuildModuleStatus>(
            guild.Modules.SingleOrDefault(x => x.ModuleRef.Id.Equals(module.Id)));
    }
}