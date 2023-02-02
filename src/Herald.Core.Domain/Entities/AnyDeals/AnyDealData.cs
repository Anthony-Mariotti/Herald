using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealData
{
    [JsonPropertyName("data")]
    public AnyDealDataList Data { get; set; } = default!;
}
