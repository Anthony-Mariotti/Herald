using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using Herald.Bot.Audio.Abstractions;
using Lavalink4NET.Rest;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

[SlashCommandGroup("queue", "Manage the track queue")]
public class SoundtrackQueueCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackQueueCommand> _logger;

    public SoundtrackQueueCommand(ILoggerFactory logger, IHeraldAudio audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackQueueCommand>();
    }

    [SlashCommand("list", "View the current track queue.")]
    public async Task QueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Queue Command Executed by {User} in {Guild}", context.User.Id,
            context.Guild.Id);
        
        //if (!await CommandPreCheckAsync(context))
        //    return;

        var modal = new DiscordInteractionResponseBuilder()
            .WithTitle("Current Queue")
            .WithCustomId("queue-list")
            .AddComponents(new TextInputComponent("Search", "search-track", "Never Gonna Give You Up", required: true));

        await context.CreateResponseAsync(InteractionResponseType.Modal, modal);

        var interactivity = context.Client.GetInteractivity();
        
        var response = await interactivity.WaitForModalAsync("queue-list", user: context.User, timeoutOverride: TimeSpan.FromSeconds(30));
        
        if (!response.TimedOut)
        {
            var inter = response.Result.Interaction;
            await inter.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent("Success"));
        }
        else
            await context.Channel.SendMessageAsync("Request timed out");
    }

    [SlashCommand("clear", "Clear out the current track queue")]
    public async Task ClearQueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Clear Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);
        
        if (!await CommandPreCheckAsync(context))
            return;
        
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Queue Clear Command"));
    }

    [SlashCommand("play", "Start playing tracks from the queue")]
    public async Task PlayQueueCommand(InteractionContext context)
    {
        _logger.LogInformation("Play Queue Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);

        if (!await CommandPreCheckAsync(context))
            return;

        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent("TODO: Queue Play Command"));
    }

    [SlashCommand("add", "Add tracks to the queue")]
    public async Task QueueAddCommand(InteractionContext context,
        [Option("search", "Track title or url")] string search = "")
    {
        try
        {
            _logger.LogInformation("Queue Add Executed by {User} in {Guild}", context.User.Username,
                context.Guild.Name);
            
            if (!await CommandPreCheckAsync(context)) return;

            if (string.IsNullOrWhiteSpace(search))
            {
                var unique = Guid.NewGuid();

                var modal = new DiscordInteractionResponseBuilder()
                    .WithTitle("Add Track to the Queue")
                    .WithCustomId($"queue-add-modal-{unique}")
                    .AddComponents(new TextInputComponent(label: "Search", customId: $"search-track-{unique}", value: "", max_length: 32));
                await context.CreateResponseAsync(InteractionResponseType.Modal, modal);

                var interactivity = context.Client.GetInteractivity();
                var response = await interactivity.WaitForModalAsync($"queue-add-modal-{unique}", timeoutOverride: TimeSpan.FromSeconds(30));

                if (!response.TimedOut)
                {
                    var inter = response.Result.Interaction;
                    var modalSearch = response.Result.Values[$"search-track-{unique}"];
                    
                    await HeraldAudio.EnqueueAsync(inter, modalSearch, SearchMode.YouTube);
                    return;
                }
                
                await context.Channel.SendMessageAsync("Request timed out");
                return;
            }
            
            await HeraldAudio.EnqueueAsync(context, search, SearchMode.YouTube);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling slash command");
        }
    }
}