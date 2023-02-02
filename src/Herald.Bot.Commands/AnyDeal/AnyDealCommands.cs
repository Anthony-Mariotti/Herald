using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.AnyDeal.Abstractions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.AnyDeal;

[SlashCommandGroup("anydeal", "See if there is any deal for a particular game, bundle, etc on isthereanydeal.com")]
public class AnyDealCommands
{
    private readonly ILogger<AnyDealCommands> _logger;
    private readonly IHeraldAnyDeal _anyDeal;

    public AnyDealCommands(ILoggerFactory logger, IHeraldAnyDeal anyDeal, ISender mediator)
    {
        _logger = logger.CreateLogger<AnyDealCommands>();
        _anyDeal = anyDeal;
    }

    [SlashCommand("search", "Search for a particular game")]
    public async Task Search(
        InteractionContext context,
        [Option("title", "title of the item you would like to look for")] string title = "")
    {
        try
        {
            _logger.LogInformation("AnyDeal Search Command Executed by {User} in {Guild}", context.User.Id, context.Guild.Id);

            var result = await _anyDeal.FindAsync(title);

            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent(string.Join(',', result.Results.Select(x => x.Title))));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error handling slash command");
        }
    }
}
