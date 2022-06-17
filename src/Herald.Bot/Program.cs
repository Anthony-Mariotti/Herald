using Herald.Bot;
using Herald.Bot.Extensions;
using Herald.Bot.Commands;
using Herald.Core;
using Herald.Bot.Events;
using Herald.Core.Application;
using Herald.Core.Infrastructure;
using Herald.Core.Infrastructure.Persistence;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddHeraldCore();
        services.AddHeraldApplicationServices();
        services.AddHeraldInfrastructure(context.Configuration);
        
        services.AddHeraldEvents();
        services.AddHeraldCommands();
        services.AddDiscordClient();
    })
    .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
    .Build();

using (var scope = host.Services.CreateScope())
{
    var initializer = scope.ServiceProvider.GetRequiredService<HeraldDbInitializer>();
    await initializer.InitializeAsync();
    await initializer.SeedAsync();
}

await host.RunAsync();