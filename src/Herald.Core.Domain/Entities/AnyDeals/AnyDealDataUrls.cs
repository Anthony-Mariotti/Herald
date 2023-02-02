using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealDataUrls
{
    [JsonPropertyName("search")]
    public string Search { get; set; } = default!;
}