using System.Reflection;
using DSharpPlus;
using DSharpPlus.Lavalink;
using Herald.Core.Configuration;

namespace Herald.Bot;

public class Worker : BackgroundService
{
    private readonly string _version;
    
    private readonly ILogger<Worker> _logger;

    private readonly DiscordClient _discord;

    private readonly HeraldConfig _config;
    
    public Worker(ILogger<Worker> logger, DiscordClient discord, HeraldConfig config)
    {
        var assembly = Assembly.GetExecutingAssembly();
        _version = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion ?? "1.0.0.0";
        _logger = logger;
        _discord = discord;
        _config = config;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Starting Herald v{Version}", _version);

        return base.StartAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var lavalink = _discord.UseLavalink();
        
        await _discord.ConnectAsync();
        await lavalink.ConnectAsync(_config.Lavalink);
    }

    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Shutting down Herald");

        _discord.DisconnectAsync();
        
        return base.StopAsync(cancellationToken);
    }
}