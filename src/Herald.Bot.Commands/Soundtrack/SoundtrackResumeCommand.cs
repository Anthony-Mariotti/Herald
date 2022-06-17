using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Herald.Core.Application.Soundtracks.Commands.ResumeTrack;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackResumeCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackResumeCommand> _logger;
    
    public SoundtrackResumeCommand(ILoggerFactory logger, ISender mediator)
        : base(logger, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackResumeCommand>();
    }
    
    [SlashCommand("resume", "Resume a paused track")]
    public async Task ResumeCommand(InteractionContext context)
    {
        _logger.LogInformation("Soundtrack Resume Command Executed by {User} in {Guild}", context.Guild.Name, context.User.Username);

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

        await GuildConnection.ResumeAsync();

        await context.CreateResponseAsync(ResumeEmbed(context.Member));
        
        await Mediator.Send(new ResumeTrackCommand(GuildConnection.Guild.Id));
    }
    
    private static DiscordEmbed ResumeEmbed(DiscordUser user)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Music has resumed")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
}