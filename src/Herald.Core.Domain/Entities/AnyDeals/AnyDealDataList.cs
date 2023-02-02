using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealDataList
{
    [JsonPropertyName("results")]
    public List<AnyDealDataPlain> Results { get; set; } = new();

    [JsonPropertyName("urls")]
    public AnyDealDataUrls Urls { get; set; } = default!;
}
