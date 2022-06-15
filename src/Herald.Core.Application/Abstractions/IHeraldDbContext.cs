using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Modules;
using Herald.Core.Domain.Entities.Soundtracks;
using MongoDB.Driver;

namespace Herald.Core.Application.Abstractions;

public interface IHeraldDbContext
{
    public IMongoCollection<GuildEntity> Guilds { get; }
    
    public IMongoCollection<SoundtrackQueueEntity> SoundtrackQueues { get; }
    
    public IMongoCollection<ModuleEntity> Modules { get; }
}