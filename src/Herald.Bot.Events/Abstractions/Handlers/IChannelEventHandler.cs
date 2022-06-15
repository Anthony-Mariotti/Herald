using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Herald.Bot.Events.Abstractions.Handlers;

public interface IChannelEventHandler
{
    public Task OnChannelCreated(DiscordClient client, ChannelCreateEventArgs args);

    public Task OnChannelUpdated(DiscordClient client, ChannelUpdateEventArgs args);

    public Task OnChannelDeleted(DiscordClient client, ChannelDeleteEventArgs args);

    public Task OnChannelPinsUpdated(DiscordClient client, ChannelPinsUpdateEventArgs args);
}