using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.Audio.Abstractions;
using Herald.Core.Application.Guilds.Queries.GetGuildModuleStatus;
using Herald.Core.Domain.ValueObjects.Modules;
using Herald.Core.Utility;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.Soundtrack;

public class SoundtrackBaseCommand : ApplicationCommandModule
{
    private readonly ILogger<SoundtrackBaseCommand> _logger;

    protected IHeraldAudio HeraldAudio { get; }

    private ISender Mediator { get; }

    protected SoundtrackBaseCommand(ILoggerFactory logger, IHeraldAudio audio, ISender mediator)
    {
        _logger = logger.CreateLogger<SoundtrackBaseCommand>();
        HeraldAudio = audio;
        Mediator = mediator;
    }

    protected async Task<bool> CommandPreCheckAsync(InteractionContext context)
    {
        _logger.LogTrace("Running Soundtrack Pre-Check for {Guild}", context.Guild.Id);
        
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

    protected static Task SendErrorResponse(BaseContext context, string title, string message) =>
        context.CreateResponseAsync(HeraldEmbedBuilder
            .Error()
            .WithTitle(title)
            .WithDescription(message)
            .Build());
}