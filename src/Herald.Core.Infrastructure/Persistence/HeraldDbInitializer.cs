using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Modules;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace Herald.Core.Infrastructure.Persistence;

public class HeraldDbInitializer
{
    private readonly ILogger<HeraldDbInitializer> _logger;
    private readonly IHeraldDbContext _context;

    public HeraldDbInitializer(ILogger<HeraldDbInitializer> logger, IHeraldDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        var modules = new List<ModuleEntity>
        {
            ModuleEntity.Create("Soundtrack")
        };

        var filter = Builders<ModuleEntity>.Filter
            .In(e => e.Name, modules.Select(x => x.Name));

        var foundModules = await _context.Modules.Find(filter)
            .Project(x => x.Name).ToListAsync();

        var modulesToInsert = modules.Where(x => !foundModules.Contains(x.Name)).ToList();

        if (modulesToInsert.Any())
        {
            await _context.Modules.InsertManyAsync(modulesToInsert);
        }
    }
}