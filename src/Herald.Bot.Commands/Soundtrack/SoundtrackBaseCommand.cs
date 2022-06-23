using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Audio.Player;
using Herald.Bot.Commands.Utilities;
using Herald.Core.Application.Guilds.Queries.GetGuildModuleStatus;
using Herald.Core.Domain.ValueObjects.Modules;
using Lavalink4NET;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackBaseCommand : ApplicationCommandModule
{
    private readonly ILogger<SoundtrackBaseCommand> _logger;
    private readonly HeraldPlayer _heraldPlayer;
    
    protected IAudioService AudioService { get; }
    
    protected ISender Mediator { get; }

    protected SoundtrackBaseCommand(ILoggerFactory logger, IAudioService audioService, ISender mediator)
    {
        _logger = logger.CreateLogger<SoundtrackBaseCommand>();
        AudioService = audioService;
        Mediator = mediator;

        _heraldPlayer = new HeraldPlayer(logger, mediator);
    }

    protected async Task<bool> CommandPreCheckAsync(InteractionContext context)
    {
        if (!await IsModuleEnabled(context)) return false;

        if (context.Member.VoiceState?.Channel is not null) return true;
        
        await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithTitle("Invalid usage")
            .WithContent("You are not in a voice channel."));
        return false;
    }

    private async Task<bool> IsModuleEnabled(BaseContext context)
    {
        var status = await Mediator.Send(new GetGuildModuleStatusQuery
        {
            GuildId = context.Guild.Id,
            Module = HeraldModule.Soundtrack
        });

        if (!status)
        {
            await context.CreateResponseAsync(HeraldEmbedBuilder
                .Error()
                .WithTitle("Module disabled!")
                .WithDescription("The Soundtrack module is not enabled in this server.")
                .Build()
            );
        }
        
        return status;
    }

    protected Task<HeraldPlayer> GetPlayerAsync(InteractionContext context)
    {
        HeraldPlayer? player = default;
        
        if (AudioService.HasPlayer(context.Guild.Id))
            player = AudioService.GetPlayer<HeraldPlayer>(context.Guild.Id);

        if (player is not null)
            return Task.FromResult(player);

        return AudioService.JoinAsync(() => _heraldPlayer, context.Guild.Id,
            context.Member.VoiceState.Channel.Id);
    }

    protected static Task SendErrorResponse(BaseContext context, string title, string message) =>
        context.CreateResponseAsync(HeraldEmbedBuilder
            .Error()
            .WithTitle(title)
            .WithDescription(message)
            .Build());
}