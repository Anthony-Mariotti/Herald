using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealMeta
{
    [JsonPropertyName("region")]
    public string Region { get; set; } = default!;

    [JsonPropertyName("country")]
    public string Country { get; set; } = default!;

    [JsonPropertyName("currency")]
    public string Currency { get; set; } = default!;
}
