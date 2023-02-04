using DSharpPlus.SlashCommands;
using Herald.Bot.Audio.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackPauseCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackPauseCommand> _logger;

    public SoundtrackPauseCommand(ILoggerFactory logger, IHeraldAudio audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackPauseCommand>();
    }

    [SlashCommand("pause", "Pause the currently playing track.")]
    public async Task PauseCommand(InteractionContext context)
    {
        try
        {
            _logger.LogInformation("Pause Command Executed by {User} in {Guild}", context.User.Id,
                context.Guild.Id);

            if (!await CommandPreCheckAsync(context))
            {
                return;
            }

            await HeraldAudio.PauseAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling slash command");
        }
    }
}