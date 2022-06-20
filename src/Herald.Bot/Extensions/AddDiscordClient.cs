using DSharpPlus;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands;
using Herald.Bot.Events.Abstractions.Handlers;

namespace Herald.Bot.Extensions;

public static partial class HeraldBotExtensions
{
    public static IServiceCollection AddDiscordClient(this IServiceCollection services)
        => services
            .AddSingleton(provider =>
            {
                var config = provider.GetRequiredService<DiscordConfiguration>();
                var client = new DiscordClient(config);
                
                RegisterDiscordGuildEvents(provider, ref client);
                RegisterDiscordChannelEvents(provider, ref client);
                RegisterDiscordMessageEvents(provider, ref client);
                RegisterCommands(provider, ref client);
                
                return client;
            });

    private static void RegisterDiscordChannelEvents(IServiceProvider provider, ref DiscordClient client)
    {
        var channelHandler = provider.GetRequiredService<IChannelEventHandler>();

        client.ChannelCreated += channelHandler.OnChannelCreated;
        client.ChannelUpdated += channelHandler.OnChannelUpdated;
        client.ChannelDeleted += channelHandler.OnChannelDeleted;
        client.ChannelPinsUpdated += channelHandler.OnChannelPinsUpdated;

    }
    
    private static void RegisterDiscordGuildEvents(IServiceProvider provider, ref DiscordClient client)
    {
        var guildHandler = provider.GetRequiredService<IGuildEventHandler>();

        client.GuildCreated += guildHandler.OnGuildCreated;
        client.GuildAvailable += guildHandler.OnGuildAvailable;
        client.GuildUpdated += guildHandler.OnGuildUpdated;
        client.GuildDeleted += guildHandler.OnGuildDeleted;
        client.GuildUnavailable += guildHandler.OnGuildUnavailable;
        client.GuildDownloadCompleted += guildHandler.OnGuildDownloadCompleted;
        client.GuildEmojisUpdated += guildHandler.OnGuildEmojisUpdated;
        client.GuildStickersUpdated += guildHandler.OnGuildStickersUpdated;
        client.GuildIntegrationsUpdated += guildHandler.OnGuildIntegrationsUpdated;
        client.GuildBanAdded += guildHandler.OnGuildBanAdded;
        client.GuildBanRemoved += guildHandler.OnGuildBanRemoved;
        client.GuildMemberAdded += guildHandler.OnGuildMemberAdded;
        client.GuildMemberRemoved += guildHandler.OnGuildMemberRemoved;
        client.GuildMemberUpdated += guildHandler.OnGuildMemberUpdated;
        client.GuildMembersChunked += guildHandler.OnGuildMembersChunked;
        client.GuildRoleCreated += guildHandler.OnGuildRoleCreated;
        client.GuildRoleUpdated += guildHandler.OnGuildRoleUpdated;
        client.GuildRoleDeleted += guildHandler.OnGuildRoleDeleted;
    }
    
    private static void RegisterDiscordMessageEvents(IServiceProvider provider, ref DiscordClient client)
    {
        var messageHandler = provider.GetRequiredService<IMessageEventHandler>();

        client.MessageAcknowledged += messageHandler.OnMessageAcknowledged;
        client.MessageCreated += messageHandler.OnMessageCreated;
        client.MessageUpdated += messageHandler.OnMessageUpdated;
        client.MessageDeleted += messageHandler.OnMessageDeleted;
        client.MessageReactionAdded += messageHandler.OnMessageReactionAdded;
        client.MessageReactionRemoved += messageHandler.OnMessageReactionRemoved;
        client.MessageReactionRemovedEmoji += messageHandler.OnMessageReactionRemovedEmoji;
        client.MessageReactionsCleared += messageHandler.OnMessageReactionsCleared;
        client.MessagesBulkDeleted += messageHandler.OnMessagesBulkDeleted;
    }

    private static void RegisterCommands(IServiceProvider provider, ref DiscordClient client)
    {
        var slash = client.UseSlashCommands(new SlashCommandsConfiguration
        {
            Services = provider
        });
                
        slash.RegisterCommands(typeof(DependencyInjection).Assembly);
    }
}