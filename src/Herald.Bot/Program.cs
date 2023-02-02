using DSharpPlus;
using Herald.Bot;
using Herald.Bot.Audio;
using Herald.Bot.Extensions;
using Herald.Bot.Commands;
using Herald.Core;
using Herald.Bot.Events;
using Herald.Core.Application;
using Herald.Core.Configuration;
using Herald.Core.Infrastructure.Common.Extensions;
using Herald.Core.Infrastructure.Persistence;
using Serilog;
using Herald.Bot.AnyDeal;

var host = Host.CreateDefaultBuilder(args)
    .ConfigureServices((context, services) =>
    {
        services.AddHostedService<Worker>();
        services.AddHeraldCore();
        services.AddHeraldApplicationServices();
        services.AddHeraldInfrastructure(context.Configuration);
        services.AddHeraldEvents();
        services.AddHeraldCommands();
        services.AddHeraldAnyDeal();
        services.AddSingleton<DiscordClient>();
        services.AddSingleton(provider =>
        {
            var config = provider.GetRequiredService<HeraldConfig>();
            var loggingFactory = provider.GetRequiredService<ILoggerFactory>();

            return new DiscordConfiguration
            {
                Token = config.DiscordKey,
                TokenType = TokenType.Bot,
                LoggerFactory = loggingFactory,
                Intents = DiscordIntents.AllUnprivileged
            };
        });
        
        services.AddDiscordClient();
        services.AddAudioServices();
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