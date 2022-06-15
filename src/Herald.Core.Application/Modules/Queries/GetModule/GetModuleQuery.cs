using AutoMapper;
using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Modules;
using MediatR;
using MongoDB.Driver;

namespace Herald.Core.Application.Modules.Queries.GetModule;

public record GetModuleQuery(string Name) : IRequest<Module?>;

public class GetModuleQueryHandler : IRequestHandler<GetModuleQuery, Module?>
{
    private readonly IHeraldDbContext _context;
    private readonly IMapper _mapper;

    public GetModuleQueryHandler(IHeraldDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }
    
    public async Task<Module?> Handle(GetModuleQuery request, CancellationToken cancellationToken)
    {
        var filter = Builders<ModuleEntity>.Filter
            .Eq(x => x.Name, request.Name);

        var module = await _context.Modules.Find(filter)
            .Limit(1)
            .SingleOrDefaultAsync(cancellationToken);

        return _mapper.Map<Module>(module);
    }
}