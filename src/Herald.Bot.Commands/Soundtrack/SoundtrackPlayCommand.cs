using DSharpPlus.SlashCommands;
using Herald.Bot.Audio.Abstractions;
using Lavalink4NET.Rest;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackPlayCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackPlayCommand> _logger;
    
    public SoundtrackPlayCommand(ILoggerFactory logger, IHeraldAudio audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackPlayCommand>();
    }
    
    [SlashCommand("play", "Play or add a track from the current queue.")]
    public async Task PlayCommand(
        InteractionContext context,
        [Option("search", "Track title or url")] string search = "")
    {
        try
        {
            _logger.LogInformation("Soundtrack Play Command Executed by {User} in {Guild}", context.User.Id,
                context.Guild.Id);

            if (!await CommandPreCheckAsync(context))
            {
                return;
            }

            await HeraldAudio.PlayAsync(context, search, SearchMode.YouTube);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling slash command");
        }
    }
}