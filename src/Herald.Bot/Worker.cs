using System.Reflection;
using DSharpPlus;
using Lavalink4NET;

namespace Herald.Bot;

public class Worker : BackgroundService
{
    private readonly string _version;
    
    private readonly ILogger<Worker> _logger;

    private readonly DiscordClient _discord;
    
    private readonly IAudioService _audioService;

    public Worker(ILogger<Worker> logger, DiscordClient discord, IAudioService audioService)
    {
        var assembly = Assembly.GetExecutingAssembly();
        _version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0.0";
        _logger = logger;
        _discord = discord;
        _audioService = audioService;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Herald v{Version}", _version);

        _discord.Ready += (_, _) => _audioService.InitializeAsync(); 
        
        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Connecting to Discord");
        await _discord.ConnectAsync();
        
        _logger.LogInformation("Herald is now connected and ready");
    }

    public override async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Shutting down Herald");

        _audioService.Dispose();
        await _discord.DisconnectAsync();

        await base.StopAsync(cancellationToken);
    }
}