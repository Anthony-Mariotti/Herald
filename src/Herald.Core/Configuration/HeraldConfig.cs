using DSharpPlus.Lavalink;
using DSharpPlus.Net;

namespace Herald.Core.Configuration;

public class HeraldConfig
{
    public string? DiscordKey { get; set; }

    public LavalinkConfiguration Lavalink { get; set; } = new LavalinkConfiguration
    {
        RestEndpoint = new ConnectionEndpoint
        {
            Hostname = "localhost",
            Port = 2333
        },
        SocketEndpoint = new ConnectionEndpoint
        {
            Hostname = "localhost",
            Port = 2333
        },
        Password = "youshallnotpass"
    };

    public HeraldDbConfig Database { get; set; } = new HeraldDbConfig();
}