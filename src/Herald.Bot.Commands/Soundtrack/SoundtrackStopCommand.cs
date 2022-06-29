using DSharpPlus.SlashCommands;
using Herald.Bot.Audio.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackStopCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackStopCommand> _logger;

    public SoundtrackStopCommand(ILoggerFactory logger, IHeraldAudio audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackStopCommand>();
    }

    [SlashCommand("stop", "Stop playing the current track and disconnects the bot from the voice channel.")]
    public async Task StopCommand(InteractionContext context)
    {
        try
        {
            _logger.LogInformation("Stop Command Executed by {User} in {Guild}", context.User.Username,
                context.Guild.Name);

            if (!await CommandPreCheckAsync(context)) return;

            await HeraldAudio.StopAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling slash command");
        }
    }
}