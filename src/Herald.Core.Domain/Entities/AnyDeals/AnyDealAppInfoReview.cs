using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealAppInfoReview
{
    [JsonPropertyName("perc_positive")]
    public int PercentPositive { get; set; }

    [JsonPropertyName("total")]
    public long Total { get; set; }

    [JsonPropertyName("text")]
    public string Text { get; set; } = default!;
}
