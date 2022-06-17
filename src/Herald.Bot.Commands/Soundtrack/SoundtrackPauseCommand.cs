using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Commands.Utilities;
using Herald.Core.Application.Soundtracks.Commands.PauseTrack;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackPauseCommand : SoundtrackBaseCommand
{
    private readonly ILogger<SoundtrackPauseCommand> _logger;

    public SoundtrackPauseCommand(ILoggerFactory logger, ISender mediator) : base(logger, mediator)
    {
        _logger = logger.CreateLogger<SoundtrackPauseCommand>();
    }

    [SlashCommand("pause", "Pause the currently playing track.")]
    public async Task PauseCommand(InteractionContext context)
    {
        _logger.LogInformation("Pause Command Executed by {User} in {Guild}", context.User.Username, context.Guild.Name);

        if (!await CommandPreCheckAsync(context)) return;

        if (!GuildConnection.IsConnected || 
            GuildConnection.CurrentState.CurrentTrack is null)
        {
            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("Not Connected")
                .WithContent("Herald is not currently playing anything."));
            return;
        }
        
        await GuildConnection.PauseAsync();

        await context.CreateResponseAsync(PauseEmbed(context.Member));
        
        await Mediator.Send(new PauseTrackCommand(GuildConnection.Guild.Id));
    }
    
    private static DiscordEmbed PauseEmbed(DiscordUser user)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Music Paused")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
}