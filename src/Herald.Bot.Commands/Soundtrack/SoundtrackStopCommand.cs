using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Lavalink4NET;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackStopCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackStopCommand> _logger;

    public SoundtrackStopCommand(ILoggerFactory logger, IAudioService audio, ISender mediator)
        : base(logger, audio, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackStopCommand>();
    }

    [SlashCommand("stop", "Stop playing the current track and disconnects the bot from the voice channel.")]
    public async Task StopCommand(InteractionContext context)
    {
        _logger.LogInformation("Stop Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);

        if (!await CommandPreCheckAsync(context))
            return;

        var player = await GetPlayerAsync(context);

        await player.StopAsync(true);
        
        await context.CreateResponseAsync(StopEmbed(context.Member));
    }
    
    private static DiscordEmbed StopEmbed(DiscordUser user)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Music has stopped")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
}