using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using Herald.Bot.AnyDeal.Abstractions;
using Microsoft.Extensions.Logging;

namespace Herald.Bot.Commands.AnyDeal;

[SlashCommandGroup("anydeal", "See if there is any deal for a particular game, bundle, etc on isthereanydeal.com")]
public class AnyDealCommands : ApplicationCommandModule
{
    private readonly ILogger<AnyDealCommands> _logger;
    private readonly IHeraldAnyDeal _anyDeal;

    public AnyDealCommands(ILoggerFactory logger, IHeraldAnyDeal anyDeal)
    {
        _logger = logger.CreateLogger<AnyDealCommands>();
        _anyDeal = anyDeal;
    }

    [SlashCommand("search", "Search for a particular game")]
    public async Task AnyDealSearch(
        InteractionContext context,
        [Option("title", "The title of the item you would like to look for")] string title = "")
    {
        _logger.LogInformation(
            "{Command} command executed by {User} in {Guild}",
            nameof(AnyDealSearch),
            context.User.Id,
            context.Guild.Id);
        try
        {
            var result = await _anyDeal.FindAsync(title);

            await context.CreateResponseAsync(new DiscordInteractionResponseBuilder().WithContent(string.Join(',', result.Results.Select(x => x.Title))));
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error handling {Command} slash command", nameof(AnyDealSearch));
        }
    }
}
