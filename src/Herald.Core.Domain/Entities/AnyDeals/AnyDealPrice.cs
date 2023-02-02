using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealPrice
{
    [JsonPropertyName("store")]
    public string Store { get; set; } = default!;

    [JsonPropertyName("cut")]
    public int Cut { get; set; }

    [JsonPropertyName("price")]
    public double Price { get; set; }

    [JsonPropertyName("price_formatted")]
    public string PriceFormatted { get; set; } = default!;

    [JsonPropertyName("url")]
    public string Url { get; set; } = default!;

    [JsonPropertyName("drm")]
    public List<string> Distributors { get; set; } = default!;
}