using DSharpPlus.Entities;

namespace Herald.Bot.Commands.Utilities;

public static class HeraldEmbedBuilder
{
    public static DiscordEmbedBuilder Success()
    {
        var builder = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.DarkGreen);

        return builder;
    }

    public static DiscordEmbedBuilder Information()
    {
        var builder = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Cyan);

        return builder;
    }

    public static DiscordEmbedBuilder Warning()
    {
        var builder = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Yellow);

        return builder;
    }

    public static DiscordEmbedBuilder Error()
    {
        var builder = new DiscordEmbedBuilder()
            .WithColor(DiscordColor.Red);

        return builder;
    }
}