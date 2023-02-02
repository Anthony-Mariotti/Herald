using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealBundles
{
    [JsonPropertyName("count")]
    public int Count { get; set; }

    [JsonPropertyName("live")]
    public List<object> Live { get; set; } = default!;
}