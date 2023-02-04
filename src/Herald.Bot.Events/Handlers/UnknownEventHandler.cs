using DSharpPlus;
using DSharpPlus.EventArgs;
using Herald.Bot.Events.Abstractions.Handlers;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Events.Handlers;

public class UnknownEventHandler : IUnknownEventHandler
{
    private readonly ILogger<UnknownEventHandler> _logger;

    public UnknownEventHandler(ILogger<UnknownEventHandler> logger)
    {
        _logger = logger;
    }

    public Task OnUnknownEvent(DiscordClient client, UnknownEventArgs args)
    {
        // It's being duplicated by DSharpPlus
        _logger.LogDebug("Unknown Event: {EventName}", args.EventName);
        args.Handled = true;

        return Task.CompletedTask;
    }
}
