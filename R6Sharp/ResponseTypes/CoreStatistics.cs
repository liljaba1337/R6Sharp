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
        #region Casual
        [JsonPropertyName("casualpvp_kills:infinite")]
        public int CasualKills { get; set; }

        [JsonPropertyName("casualpvp_death:infinite")]
        public int CasualDeaths { get; set; }

        [JsonPropertyName("casualpvp_matchplayed:infinite")]
        public int CasualMatchesPlayed { get; set; }

        [JsonPropertyName("casualpvp_matchwon:infinite")]
        public int CasualMatchesWon { get; set; }

        [JsonPropertyName("casualpvp_matchlost:infinite")]
        public int CasualMatchesLost { get; set; }

        [JsonPropertyName("casualpvp_timeplayed:infinite")]
        public long CasualTimePlayed { get; set; }
        #endregion

        #region Ranked
        [JsonPropertyName("rankedpvp_kills:infinite")]
        public int RankedKills { get; set; }

        [JsonPropertyName("rankedpvp_death:infinite")]
        public int RankedDeaths { get; set; }

        [JsonPropertyName("rankedpvp_matchwon:infinite")]
        public int RankedMatchesWon { get; set; }

        [JsonPropertyName("rankedpvp_matchlost:infinite")]
        public int RankedMatchesLost { get; set; }

        [JsonPropertyName("rankedpvp_matchplayed:infinite")]
        public int RankedMatchesPlayed { get; set; }

        [JsonPropertyName("rankedpvp_timeplayed:infinite")]
        public long RankedTimePlayed { get; set; }
        #endregion

        #region Training
        [JsonPropertyName("generalpve_kills:infinite")]
        public int TrainingKills { get; set; }

        [JsonPropertyName("generalpve_death:infinite")]
        public int TrainingDeaths { get; set; }

        [JsonPropertyName("generalpve_matchwon:infinite")]
        public int TrainingMatchesWon { get; set; }

        [JsonPropertyName("generalpve_matchlost:infinite")]
        public int TrainingMatchesLost { get; set; }

        [JsonPropertyName("generalpve_matchplayed:infinite")]
        public int TrainingMatchesPlayed { get; set; }

        [JsonPropertyName("generalpve_timeplayed:infinite")]
        public long TrainingTimePlayed { get; set; }
        #endregion

        #region General
        [JsonPropertyName("generalpvp_kills:infinite")]
        public int GeneralKills { get; set; }

        [JsonPropertyName("generalpvp_death:infinite")]
        public int GeneralDeaths { get; set; }

        [JsonPropertyName("generalpvp_killassists:infinite")]
        public int GeneralAssists { get; set; }

        [JsonPropertyName("generalpvp_matchwon:infinite")]
        public int GeneralMatchesWon { get; set; }

        [JsonPropertyName("generalpvp_matchlost:infinite")]
        public int GeneralMatchesLost { get; set; }

        [JsonPropertyName("generalpvp_matchplayed:infinite")]
        public int GeneralMatchesPlayed { get; set; }

        [JsonPropertyName("generalpvp_timeplayed:infinite")]
        public long GeneralTimePlayed { get; set; }

        [JsonPropertyName("generalpvp_bullethit:infinite")]
        public int GeneralBulletsHit { get; set; }

        [JsonPropertyName("generalpvp_headshot:infinite")]
        public int GeneralHeadshots { get; set; }

        [JsonPropertyName("generalpvp_barricadedeployed:infinite")]
        public int GeneralBarricades { get; set; }

        [JsonPropertyName("generalpvp_reinforcementdeploy:infinite")]
        public int GeneralReinforcements { get; set; }

        [JsonPropertyName("generalpvp_dbno:infinite")]
        public int GeneralDowns { get; set; }

        [JsonPropertyName("generalpvp_revive:infinite")]
        public int GeneralRevives { get; set; }

        [JsonPropertyName("generalpvp_suicide:infinite")]
        public int GeneralSuicides { get; set; }
        #endregion
    }
}
