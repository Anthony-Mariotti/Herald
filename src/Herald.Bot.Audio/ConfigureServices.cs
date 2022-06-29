using Herald.Bot.Audio.Abstractions;
using Herald.Bot.Audio.Player;
using Herald.Core.Configuration;
using Lavalink4NET;
using Lavalink4NET.DSharpPlus;
using Microsoft.Extensions.DependencyInjection;

namespace Herald.Bot.Audio;

public static class ConfigureServices
{
    public static IServiceCollection AddAudioServices(this IServiceCollection services)
    {
        services.AddSingleton<IDiscordClientWrapper, DiscordClientWrapper>();
        services.AddSingleton(provider =>
        {
            var config = provider.GetRequiredService<HeraldConfig>();
            return config.Lavalink;
        });
        
        services.AddSingleton<IAudioService, LavalinkNode>();

        services.AddSingleton<HeraldPlayer>();
        services.AddTransient<IHeraldAudio, HeraldAudio>();
        /*
            TODO: For some reason I can't retrieve the artwork server from Lavalink4NET
            It's possible that I am doing something wrong, but I have followed their documentation
            as much as possible.
            
            services.AddSingleton<IArtworkService, ArtworkService>();
        */
        return services;
    }
}