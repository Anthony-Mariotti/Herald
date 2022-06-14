using Herald.Bot;
using Herald.Bot.Extensions;
using Herald.Commands;
using Herald.Core;
using Herald.Events;
using Serilog;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<Worker>();
        services.AddHeraldCore();
        services.AddHeraldEvents();
        services.AddHeraldCommands();
        services.AddDiscordClient();
    })
    .UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration))
    .Build();

await host.RunAsync();