using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealApp
{
    [JsonPropertyName("price")]
    public AnyDealPrice Price { get; set; } = default!;

    [JsonPropertyName("lowest")]
    public AnyDealLowest Lowest { get; set; } = default!;

    [JsonPropertyName("bundles")]
    public AnyDealBundles Bundles { get; set; } = default!;

    [JsonPropertyName("urls")]
    public AnyDealUrls Urls { get; set; } = default!;
}