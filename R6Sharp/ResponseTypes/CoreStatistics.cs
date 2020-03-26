using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.ResponseTypes
{
    public class CoreStatisticsFetch
    {
        [JsonPropertyName("results")]
        public Dictionary<string, CoreStatistics> Core { get; set; }
    }

    public class CoreStatistics
    {
        [JsonPropertyName("casualpvp_matchwon:infinite")]
        public int CasualMatchesWon { get; set; }

        [JsonPropertyName("rankedpvp_timeplayed:infinite")]
        public long RankedTimePlayed { get; set; }

        [JsonPropertyName("rankedpvp_death:infinite")]
        public int RankedDeaths { get; set; }

        [JsonPropertyName("generalpvp_matchwon:infinite")]
        public int GeneralMatchesWon { get; set; }

        [JsonPropertyName("generalpvp_bullethit:infinite")]
        public int GeneralBulletsHit { get; set; }

        [JsonPropertyName("generalpvp_matchlost:infinite")]
        public int GeneralMatchesLost { get; set; }

        [JsonPropertyName("generalpvp_killassists:infinite")]
        public int GeneralAssists { get; set; }

        [JsonPropertyName("generalpvp_matchplayed:infinite")]
        public int GeneralMatchesPlayed { get; set; }

        [JsonPropertyName("generalpvp_timeplayed:infinite")]
        public long GeneralTimePlayed { get; set; }

        [JsonPropertyName("casualpvp_matchplayed:infinite")]
        public int CasualMatchesPlayed { get; set; }

        [JsonPropertyName("casualpvp_death:infinite")]
        public int CasualDeaths { get; set; }

        [JsonPropertyName("generalpvp_kills:infinite")]
        public int GeneralKills { get; set; }

        [JsonPropertyName("casualpvp_kills:infinite")]
        public int CasualKills { get; set; }

        [JsonPropertyName("generalpvp_death:infinite")]
        public int GeneralDeaths { get; set; }

        [JsonPropertyName("rankedpvp_matchlost:infinite")]
        public int RankedMatchesLost { get; set; }

        [JsonPropertyName("casualpvp_matchlost:infinite")]
        public int CasualMatchesLost { get; set; }

        [JsonPropertyName("casualpvp_timeplayed:infinite")]
        public long CasualTimePlayed { get; set; }

        [JsonPropertyName("rankedpvp_matchwon:infinite")]
        public int RankedMatchesWon { get; set; }

        [JsonPropertyName("generalpvp_headshot:infinite")]
        public int GeneralHeadshots { get; set; }

        [JsonPropertyName("rankedpvp_matchplayed:infinite")]
        public int RankedMatchesPlayed { get; set; }

        [JsonPropertyName("rankedpvp_kills:infinite")]
        public int RankedKills { get; set; }
    }
}
