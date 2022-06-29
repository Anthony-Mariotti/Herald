using DSharpPlus.Entities;
using Herald.Core.Domain.ValueObjects.Soundtracks;
using Herald.Core.Utility;
using Humanizer;
using Humanizer.Localisation;
using Lavalink4NET.Player;

namespace Herald.Bot.Audio;

public static class HeraldAudioMessage
{
    public static DiscordEmbed TrackNotFoundEmbed(string search, DiscordUser user)
        => HeraldEmbedBuilder
            .Warning()
            .WithAuthor("Track Not Found")
            .AddField("Search", search)
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
    
    public static DiscordEmbed NowPlayingEmbed(LavalinkTrack track, DiscordUser user)
        =>  HeraldEmbedBuilder
            .Information()
            .WithAuthor("Now Playing", iconUrl: "https://play-lh.googleusercontent.com/SqMGe5wxL6HfT03WNGepMvGxXyS9EOFm4V7NzLCofFxPwFVJqRavYe5-EPQV3WAW7DU")
            .AddField("Title", $"[{track.Title}]({track.Source})", true)
            .AddField("Duration", $"{track.Duration.Humanize(minUnit: TimeUnit.Second, precision: 3)}", true)
            .WithImageUrl($"https://img.youtube.com/vi/{track.TrackIdentifier}/0.jpg")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();

    public static DiscordEmbed AddedToQueueEmbed(LavalinkTrack track, DiscordUser user)
        => HeraldEmbedBuilder
            .Success()
            .WithAuthor("Added track to queue!", iconUrl: "https://upload.wikimedia.org/wikipedia/commons/thumb/3/3b/Eo_circle_green_checkmark.svg/2048px-Eo_circle_green_checkmark.svg.png")
            .AddField("Title", $"[{track.Title}]({track.Source})", true)
            .AddField("Duration", $"{track.Duration.Humanize(minUnit: TimeUnit.Second, precision: 3)}", true)
            .WithImageUrl($"https://img.youtube.com/vi/{track.TrackIdentifier}/0.jpg")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();

    public static DiscordEmbed TrackDequeuedEmbed(QueuedTrackValue track, DiscordUser user)
        => HeraldEmbedBuilder
            .Information()
            .WithAuthor("Track removed from queue!")
            .AddField("Title", $"[{track.Title}]({track.Source})", true)
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();

    public static DiscordEmbed StoppedEmbed(DiscordUser user)
        => HeraldEmbedBuilder
            .Information()
            .WithAuthor("Stopped Playing!")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
    
    public static DiscordEmbed PausedEmbed(DiscordUser user)
        => HeraldEmbedBuilder
            .Information()
            .WithAuthor("Paused Track!")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();

    public static DiscordEmbed ResumedEmbed(LavalinkTrack track, DiscordUser user)
        => HeraldEmbedBuilder
            .Information()
            .WithAuthor("Resumed Track!")
            .AddField("Title", $"[{track.Title}]({track.Source})", true)
            .AddField("Remaining",
                $"{(track.Duration - track.Position).Humanize(minUnit: TimeUnit.Second, precision: 3)}", true)
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();

    public static DiscordEmbed NoTrackToResumeEmbed(DiscordUser user)
        => HeraldEmbedBuilder
            .Error()
            .WithAuthor("No track to resume!")
            .WithFooter($"Requested by {user.Username}#{user.Discriminator}", user.AvatarUrl)
            .WithTimestamp(DateTime.Now)
            .Build();
}