using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealAppInfo
{
    [JsonPropertyName("title")]
    public string Title { get; set; } = default!;

    [JsonPropertyName("greenlight")]
    public AnyDealAppInfoGreenlight Greenlight { get; set; } = default!;

    [JsonPropertyName("is_package")]
    public bool IsPackage { get; set; }

    [JsonPropertyName("is_dlc")]
    public bool IsDlc { get; set; }

    [JsonPropertyName("achievements")]
    public bool HasAchievements { get; set; }

    [JsonPropertyName("trading_cards")]
    public bool HasTradingCards { get; set; }

    [JsonPropertyName("early_access")]
    public bool IsEarlyAccess { get; set; }

    [JsonPropertyName("reviews")]
    public Dictionary<string, AnyDealAppInfoReview> Reviews { get; set; } = default!;

    [JsonPropertyName("urls")]
    public AnyDealAppInfoUrls Urls { get; set; } = default!;
}
