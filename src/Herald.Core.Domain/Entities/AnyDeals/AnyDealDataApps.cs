using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealDataApps
{
    [JsonPropertyName(".meta")]
    public AnyDealMeta Metadata { get; set; } = default!;

    [JsonPropertyName("data")]
    public Dictionary<string, AnyDealApp> Apps { get; set; } = default!;
}
