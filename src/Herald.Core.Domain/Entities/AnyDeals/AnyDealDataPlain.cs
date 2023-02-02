using System.Text.Json.Serialization;

namespace Herald.Core.Domain.Entities.AnyDeals;

public class AnyDealDataPlain
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("plain")]
    public string Plain { get; set; } = default!;

    [JsonPropertyName("title")]
    public string Title { get; set; } = default!;
}
