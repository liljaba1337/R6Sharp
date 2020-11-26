using System.Text.Json.Serialization;
using static R6Sharp.Parser;

namespace R6Sharp.Response.Statistic
{
    public class PlayerStatistics
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(ParseStringToSpecificDataTypeType))]
        public SpecificDataType Type { get; set; }

        [JsonPropertyName("statsType")]
        [JsonConverter(typeof(ParseStringToDataTypeType))]
        public DataType StatsType { get; set; }

        [JsonPropertyName("statsDetail")]
        public string StatsDetail { get; set; }

        [JsonPropertyName("matchesPlayed")]
        public int MatchesPlayed { get; set; }

        [JsonPropertyName("roundsPlayed")]
        public int RoundsPlayed { get; set; }

        [JsonPropertyName("minutesPlayed")]
        public int MinutesPlayed { get; set; }

        [JsonPropertyName("matchesWon")]
        public int MatchesWon { get; set; }

        [JsonPropertyName("matchesLost")]
        public int MatchesLost { get; set; }

        [JsonPropertyName("roundsWon")]
        public int RoundsWon { get; set; }

        [JsonPropertyName("roundsLost")]
        public int RoundsLost { get; set; }

        [JsonPropertyName("kills")]
        public int Kills { get; set; }

        [JsonPropertyName("assists")]
        public int Assists { get; set; }

        [JsonPropertyName("death")]
        public int Death { get; set; }

        [JsonPropertyName("headshots")]
        public int Headshots { get; set; }

        [JsonPropertyName("meleeKills")]
        public int MeleeKills { get; set; }

        [JsonPropertyName("teamKills")]
        public int TeamKills { get; set; }

        [JsonPropertyName("openingKills")]
        public int OpeningKills { get; set; }

        [JsonPropertyName("openingDeaths")]
        public int OpeningDeaths { get; set; }

        [JsonPropertyName("trades")]
        public int Trades { get; set; }

        [JsonPropertyName("openingKillTrades")]
        public int OpeningKillTrades { get; set; }

        [JsonPropertyName("openingDeathTrades")]
        public int OpeningDeathTrades { get; set; }

        [JsonPropertyName("revives")]
        public int Revives { get; set; }

        [JsonPropertyName("distanceTravelled")]
        public int DistanceTravelled { get; set; }

        [JsonPropertyName("winLossRatio")]
        public double WinLossRatio { get; set; }

        [JsonPropertyName("killDeathRatio")]
        public PValue KillDeathRatio { get; set; }

        [JsonPropertyName("headshotAccuracy")]
        public PValue HeadshotAccuracy { get; set; }

        [JsonPropertyName("killsPerRound")]
        public PValue KillsPerRound { get; set; }

        [JsonPropertyName("roundsWithAKill")]
        public PValue RoundsWithAKill { get; set; }

        [JsonPropertyName("roundsWithMultiKill")]
        public PValue RoundsWithMultiKill { get; set; }

        [JsonPropertyName("roundsWithOpeningKill")]
        public PValue RoundsWithOpeningKill { get; set; }

        [JsonPropertyName("roundsWithOpeningDeath")]
        public PValue RoundsWithOpeningDeath { get; set; }

        [JsonPropertyName("roundsWithKOST")]
        public PValue RoundsWithKost { get; set; }

        [JsonPropertyName("roundsSurvived")]
        public PValue RoundsSurvived { get; set; }

        [JsonPropertyName("roundsWithAnAce")]
        public PValue RoundsWithAnAce { get; set; }

        [JsonPropertyName("roundsWithClutch")]
        public PValue RoundsWithClutch { get; set; }

        [JsonPropertyName("timeAlivePerMatch")]
        public double TimeAlivePerMatch { get; set; }

        [JsonPropertyName("timeDeadPerMatch")]
        public double TimeDeadPerMatch { get; set; }

        [JsonPropertyName("distancePerRound")]
        public double DistancePerRound { get; set; }
    }

    public class PValue
    {
        [JsonPropertyName("value")]
        public double Value { get; set; }

        [JsonPropertyName("p")]
        public double P { get; set; }
    }
}
