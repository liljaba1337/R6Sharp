using System.Text.Json.Serialization;

namespace R6Sharp.Response.Statistic
{
    public class SeasonalPlayerStatistics : PlayerStatistics
    {
        [JsonPropertyName("seasonYear")]
        public string SeasonYear { get; set; }

        [JsonPropertyName("seasonNumber")]
        public string SeasonNumber { get; set; }
    }
}
