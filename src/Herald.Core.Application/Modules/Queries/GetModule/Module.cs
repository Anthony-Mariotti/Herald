using AutoMapper;
using Herald.Core.Application.Common.Mappings;
using Herald.Core.Domain.Entities.Modules;
using MongoDB.Bson;

namespace Herald.Core.Application.Modules.Queries.GetModule;

public class Module : IMapFrom<ModuleEntity>
{
    public ObjectId Id { get; set; }

    public string Name { get; set; } = default!;
    
    public void Mapping(Profile profile)
    {
        profile.CreateMap<ModuleEntity, Module>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
            .ForMember(d => d.Name, opt => opt.MapFrom(s => s.Name));
    }
}