using Lavalink4NET;

namespace Herald.Core.Configuration;

public class HeraldConfig
{
    public string DiscordKey { get; set; } = default!;

    public LavalinkNodeOptions Lavalink { get; set; } = new LavalinkNodeOptions
    {
        RestUri = "http://localhost:2333/",
        WebSocketUri = "ws://localhost:2333/",
        Password = "youshallnotpass",
        DisconnectOnStop = true,
        AllowResuming = true,
        UserAgent = "Herald/1.0.0.0-alpha",
        Label = "local",
        ReconnectStrategy = ReconnectStrategies.DefaultStrategy
    };
}