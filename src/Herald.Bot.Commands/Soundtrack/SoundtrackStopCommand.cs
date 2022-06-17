using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Herald.Core.Application.Soundtracks.Commands.StopTrack;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackStopCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackStopCommand> _logger;

    public SoundtrackStopCommand(ILoggerFactory logger, ISender mediator)
        : base(logger, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackStopCommand>();
    }

    [SlashCommand("stop", "Stop playing the current track and disconnects the bot from the voice channel.")]
    public async Task StopCommand(InteractionContext context)
    {
        _logger.LogInformation("Stop Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);

        if (!await CommandPreCheckAsync(context))
            return;

        if (!GuildConnection.IsConnected ||
            GuildConnection.CurrentState.CurrentTrack is null)
        {
            await context.CreateResponseAsync(
                new DiscordInteractionResponseBuilder().WithContent(
                    "I'm not currently connected to any voice channel."));
            return;
        }
        
        await GuildConnection.StopAsync();
        await GuildConnection.DisconnectAsync();

        await context.CreateResponseAsync(StopEmbed(context.Member));
        
        await Mediator.Send(new StopTrackCommand(GuildConnection.Guild.Id));
    }
    
    private static DiscordEmbed StopEmbed(DiscordUser user)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Music has stopped")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
}