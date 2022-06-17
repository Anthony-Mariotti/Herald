using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Herald.Core.Infrastructure.Persistence;

public class HeraldDbInitializer
{
    private readonly ILogger<HeraldDbInitializer> _logger;
    private readonly HeraldDbContext _context;

    public HeraldDbInitializer(ILogger<HeraldDbInitializer> logger, HeraldDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public async Task InitializeAsync()
    {
        _logger.LogTrace("Initializing database");
        try
        {
            if (_context.Database.IsSqlServer())
            {
                _logger.LogTrace("Beginning database migration");
                await _context.Database.MigrateAsync();
                _logger.LogTrace("Finished database migration");
            }
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
        await _context.SaveChangesAsync();
    }
}