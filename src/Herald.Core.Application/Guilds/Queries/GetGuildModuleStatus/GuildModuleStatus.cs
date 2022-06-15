using AutoMapper;
using Herald.Core.Application.Common.Mappings;
using Herald.Core.Domain.ValueObjects.Guilds;
using MongoDB.Bson;

namespace Herald.Core.Application.Modules.Queries.GetModuleStatus;

public class GuildModuleStatus : IMapFrom<GuildModule>
{
    public ObjectId Id { get; set; }
    
    public bool Enabled { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<GuildModule, GuildModuleStatus>()
            .ForMember(d => d.Id, opt => opt.MapFrom(s => (ObjectId)s.ModuleRef.Id))
            .ForMember(d => d.Enabled, opt => opt.MapFrom(s => s.Enabled));
    }
}