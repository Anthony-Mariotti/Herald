using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealUrls
{
    [JsonPropertyName("info")]
    public string Info { get; set; } = default!;

    [JsonPropertyName("history")]
    public string History { get; set; } = default!;

    [JsonPropertyName("bundles")]
    public string Bundles { get; set; } = default!;
}