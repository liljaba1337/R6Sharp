using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static R6Sharp.Parser;

namespace R6Sharp.Response
{
    public enum DataResponseType { Maps, Operators, Summary, Trend, Weapons }

    public enum RoleType { TeamRoles, TeamRolesWeapons }

    public class DataResponse
    {
        [JsonPropertyName("profileId")]
        public Guid ProfileId { get; set; }

        [JsonPropertyName("startDate")]
        [JsonConverter(typeof(ParseStringToDateTime))]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        [JsonConverter(typeof(ParseStringToDateTime))]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("region")]
        [JsonConverter(typeof(ParseStringToRegion))]
        public Region Region { get; set; }

        [JsonPropertyName("statType")]
        [JsonConverter(typeof(ParseStringToDataResponseType))]
        public DataResponseType StatType { get; set; }

        [JsonPropertyName("platforms")]
        public Dictionary<string, Platform> Platforms { get; set; }
    }

    public class Platform
    {
        [JsonPropertyName("gameModes")]
        public Dictionary<string, Gamemode> Gamemodes { get; set; }
    }

    public class Gamemode
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(ParseStringToRoleType))]
        public RoleType Type { get; set; }

        [JsonPropertyName("teamRoles")]
        public Dictionary<string, List<Statistic>> TeamRoles { get; set; }
    }

    public class Statistic
    {
        [JsonPropertyName("assists")]
        public int Assists { get; set; }

        [JsonPropertyName("death")]
        public int Death { get; set; }

        [JsonPropertyName("distancePerRound")]
        public double DistancePerRound { get; set; }

        [JsonPropertyName("distanceTravelled")]
        public int DistanceTravelled { get; set; }

        [JsonPropertyName("headshotAccuracy")]
        public PValue HeadshotAccuracy { get; set; }

        [JsonPropertyName("headshots")]
        public int Headshots { get; set; }

        [JsonPropertyName("killDeathRatio")]
        public PValue KillDeathRatio { get; set; }

        [JsonPropertyName("kills")]
        public int Kills { get; set; }

        [JsonPropertyName("killsPerRound")]
        public PValue KillsPerRound { get; set; }

        [JsonPropertyName("matchesLost")]
        public int MatchesLost { get; set; }

        [JsonPropertyName("matchesPlayed")]
        public int MatchesPlayed { get; set; }

        [JsonPropertyName("matchesWon")]
        public int MatchesWon { get; set; }

        [JsonPropertyName("meleeKills")]
        public int MeleeKills { get; set; }

        [JsonPropertyName("minutesPlayed")]
        public int MinutesPlayed { get; set; }

        [JsonPropertyName("openingDeathTrades")]
        public int OpeningDeathTrades { get; set; }

        [JsonPropertyName("openingDeaths")]
        public int OpeningDeaths { get; set; }

        [JsonPropertyName("openingKillTrades")]
        public int OpeningKillTrades { get; set; }

        [JsonPropertyName("openingKills")]
        public int OpeningKills { get; set; }

        [JsonPropertyName("revives")]
        public int Revives { get; set; }

        [JsonPropertyName("roundsLost")]
        public int RoundsLost { get; set; }

        [JsonPropertyName("roundsPlayed")]
        public int RoundsPlayed { get; set; }

        [JsonPropertyName("roundsSurvived")]
        public PValue RoundsSurvived { get; set; }

        [JsonPropertyName("roundsWithAKill")]
        public PValue RoundsWithAKill { get; set; }

        [JsonPropertyName("roundsWithAnAce")]
        public PValue RoundsWithAnAce { get; set; }

        [JsonPropertyName("roundsWithClutch")]
        public PValue RoundsWithClutch { get; set; }

        [JsonPropertyName("roundsWithKOST")]
        public PValue RoundsWithKOST { get; set; }

        [JsonPropertyName("roundsWithMultiKill")]
        public PValue RoundsWithMultiKill { get; set; }

        [JsonPropertyName("roundsWithOpeningDeath")]
        public PValue RoundsWithOpeningDeath { get; set; }

        [JsonPropertyName("roundsWithOpeningKill")]
        public PValue RoundsWithOpeningKill { get; set; }

        [JsonPropertyName("roundsWon")]
        public int RoundsWon { get; set; }

        [JsonPropertyName("statsDetail")]
        public string StatsDetail { get; set; }

        [JsonPropertyName("statsType")]
        [JsonConverter(typeof(ParseStringToDataResponseType))]
        public DataResponseType StatsType { get; set; }

        [JsonPropertyName("teamKills")]
        public int TeamKills { get; set; }

        [JsonPropertyName("timeAlivePerMatch")]
        public double TimeAlivePerMatch { get; set; }

        [JsonPropertyName("timeDeadPerMatch")]
        public double TimeDeadPerMatch { get; set; }

        [JsonPropertyName("trades")]
        public int Trades { get; set; }

        [JsonPropertyName("type")]
        public string Type { get; set; }

        [JsonPropertyName("winLossRatio")]
        public double WinLossRatio { get; set; }
    }

    public class PValue
    {
        [JsonPropertyName("value")]
        public double Value { get; set; }

        [JsonPropertyName("p")]
        public double P { get; set; }
    }
}