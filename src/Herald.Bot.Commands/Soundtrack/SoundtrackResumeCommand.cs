using DSharpPlus.SlashCommands;
using Herald.Bot.Audio.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackResumeCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackResumeCommand> _logger;
    
    public SoundtrackResumeCommand(ILoggerFactory logger, IHeraldAudio audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackResumeCommand>();
    }
    
    [SlashCommand("resume", "Resume a paused track")]
    public async Task ResumeCommand(InteractionContext context)
    {
        try
        {
            _logger.LogInformation("Soundtrack Resume Command Executed by {User} in {Guild}", context.Guild.Name,
                context.User.Username);
            
            if (!await CommandPreCheckAsync(context)) return;

            await HeraldAudio.ResumeAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling slash command");
        }
    }
}