using Herald.Core.Application.Abstractions;
using Herald.Core.Configuration;
using Herald.Core.Domain.Entities.Guilds;
using Herald.Core.Domain.Entities.Modules;
using Herald.Core.Domain.Entities.Soundtracks;
using MongoDB.Driver;

namespace Herald.Core.Infrastructure.Persistence;

public class HeraldDbContext : IHeraldDbContext
{
    private IMongoDatabase Database { get; }

    public IMongoCollection<GuildEntity> Guilds =>
        Database.GetCollection<GuildEntity>("Guilds");

    public IMongoCollection<SoundtrackQueueEntity> SoundtrackQueues =>
        Database.GetCollection<SoundtrackQueueEntity>("SoundtrackQueue");

    public IMongoCollection<ModuleEntity> Modules =>
        Database.GetCollection<ModuleEntity>("Modules");

    public HeraldDbContext(HeraldConfig config)
    {
        var settings = MongoClientSettings.FromConnectionString(config.Database.ConnectionString);
        var client = new MongoClient(settings);
        Database = client.GetDatabase("Herald");
    }
}