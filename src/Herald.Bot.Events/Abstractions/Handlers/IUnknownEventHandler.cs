using DSharpPlus;
using DSharpPlus.EventArgs;

namespace Herald.Bot.Events.Abstractions.Handlers;

public interface IUnknownEventHandler
{
    public Task OnUnknownEvent(DiscordClient client, UnknownEventArgs args);
}
