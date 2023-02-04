using Herald.Core.Application.Abstractions;
using Herald.Core.Domain.Entities.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

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

    public async Task InitializeAsync()
    {
        _logger.LogTrace("Initializing database");
        try
        {
            _logger.LogTrace("Beginning database migration");
            await _context.Database.MigrateAsync();
            _logger.LogTrace("Finished database migration");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        _logger.LogTrace("Seeding database");
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {
        await SeedModules();
        _ = await _context.SaveChangesAsync();
    }

    private async Task SeedModules()
    {
        var existing = await _context.Modules.ToListAsync();
        var missing = Module.AvailableModules.Except(existing, new ModuleExistingComparer());
        var updated = Module.AvailableModules.Except(existing, new ModuleUpdatedComparer());
        
        if (missing.Any())
        {
            foreach(var module in missing)
            {
                _context.Entry(module).State = EntityState.Added;
            }
            await _context.Modules.AddRangeAsync(existing);
        }

        if (updated.Any())
        {
            foreach (var existingModule in existing.Where(x => updated.Select(x => x.Id).Contains(x.Id)))
            {
                var updatedModule = updated.FirstOrDefault(x => x.Id == existingModule.Id);

                if (updatedModule is not null)
                {
                    if (existingModule.Name != updatedModule.Name)
                    {
                        existingModule.SetName(updatedModule.Name);
                    }

                    if (existingModule.Released != updatedModule.Released)
                    {
                        if (updatedModule.Released)
                        {
                            existingModule.Release();
                        }
                        else
                        {
                            existingModule.UnRelease();
                        }
                    }

                    _context.Entry(existingModule).State = EntityState.Modified;
                }
            }
        }
    }
}