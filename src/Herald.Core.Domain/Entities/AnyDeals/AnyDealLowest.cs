using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealLowest
{
    [JsonPropertyName("store")]
    public string Store { get; set; } = default!;

    [JsonPropertyName("cut")]
    public int Cut { get; set; } = default!;

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("price_formatted")]
    public string PriceFormatted { get; set; } = default!;

    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;

    [JsonPropertyName("recorded")]
    public long Recorded { get; set; }

    [JsonPropertyName("recorded_formatted")]
    public string RecordedFormatted { get; set; } = default!;
}