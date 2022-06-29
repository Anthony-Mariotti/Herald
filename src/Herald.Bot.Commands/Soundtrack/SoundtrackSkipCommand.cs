using DSharpPlus.SlashCommands;
using Herald.Bot.Audio.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackSkipCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackSkipCommand> _logger;

    public SoundtrackSkipCommand(ILoggerFactory logger, IHeraldAudio audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackSkipCommand>();
    }

    [SlashCommand("skip", "Skip the current track that is currently playing")]
    public async Task SkipCommand(InteractionContext context)
    {
        try
        {
            _logger.LogInformation("Skip Command Executed by {User} in {Guild}", context.User.Id,
                context.Guild.Id);
            
            if (!await CommandPreCheckAsync(context)) return;

            await HeraldAudio.SkipAsync(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling slash command");
        }
    }
}