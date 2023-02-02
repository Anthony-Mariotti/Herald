using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealAppInfoUrls
{
    [JsonPropertyName("game")]
    public string Game { get; set; } = default!;

    [JsonPropertyName("package")]
    public string Package { get; set; } = default!;

    [JsonPropertyName("dlc")]
    public string Dlc { get; set; } = default!;
}
