using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.Response.Statistic
{
    public class ObjectiveStatisticFetch
    {
        [JsonPropertyName("results")]
        public Dictionary<string, ObjectiveStatistic> Players { get; set; }
    }

    public class ObjectiveStatistic
    {
        [JsonPropertyName("secureareapvp_matchwon:infinite")]
        public int SecureAreaMatchWon { get; set; }

        [JsonPropertyName("secureareapvp_matchlost:infinite")]
        public int SecureAreaMatchLost { get; set; }

        [JsonPropertyName("secureareapvp_matchplayed:infinite")]
        public int SecureAreaMatchPlayed { get; set; }

        [JsonPropertyName("secureareapvp_bestscore:infinite")]
        public int SecureAreaBestScore { get; set; }

        [JsonPropertyName("plantbombpvp_matchwon:infinite")]
        public int BombMatchWon { get; set; }

        [JsonPropertyName("plantbombpvp_matchlost:infinite")]
        public int BombMatchLost { get; set; }

        [JsonPropertyName("plantbombpvp_matchplayed:infinite")]
        public int BombMatchPlayed { get; set; }

        [JsonPropertyName("plantbombpvp_bestscore:infinite")]
        public int BombBestScore { get; set; }

        [JsonPropertyName("rescuehostagepvp_matchwon:infinite")]
        public int HostageMatchWon { get; set; }

        [JsonPropertyName("rescuehostagepvp_matchlost:infinite")]
        public int HostageMatchLost { get; set; }

        [JsonPropertyName("rescuehostagepvp_matchplayed:infinite")]
        public int HostageMatchPlayed { get; set; }

        [JsonPropertyName("rescuehostagepvp_bestscore:infinite")]
        public int HostageBestScore { get; set; }
    }
}
