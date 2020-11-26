using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static R6Sharp.Parser;

namespace R6Sharp.Response
{
    public class Narrative
    {
        [JsonPropertyName("narrativeType")]
        public string NarrativeType { get; set; }

        [JsonPropertyName("narrativePeriod")]
        public string NarrativePeriod { get; set; }

        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("profiles")]
        public Dictionary<string, ProfileNarrative> Profiles { get; set; }
    }

    public class ProfileNarrative
    {
        [JsonPropertyName("years")]
        public Dictionary<string, Year> Years { get; set; }
    }

    public class Year
    {
        [JsonPropertyName("weeks")]
        public Dictionary<string, Week> Weeks { get; set; }
    }

    public class Week
    {
        [JsonPropertyName("narrative")]
        public string[] Narrative { get; set; }

        [JsonPropertyName("bestMatchFullStatistics")]
        public BestMatchFullStatistics BestMatchFullStatistics { get; set; }

        [JsonPropertyName("weeklyFullPlayerStatistics")]
        public WeeklyFullPlayerStatistics WeeklyFullPlayerStatistics { get; set; }
    }

    public class BestMatchFullStatistics
    {
        [JsonPropertyName("profileId")]
        public Guid ProfileId { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("week")]
        public int Week { get; set; }

        [JsonPropertyName("score")]
        public int Score { get; set; }

        [JsonPropertyName("gamertag")]
        public string Gamertag { get; set; }

        [JsonPropertyName("gameMode")]
        public string GameMode { get; set; }

        [JsonPropertyName("bestMatchMap")]
        public string BestMatchMap { get; set; }

        [JsonPropertyName("bestMatchServerDateId")]
        [JsonConverter(typeof(ParseStringToDateTime))]
        public DateTime BestMatchServerDateId { get; set; }

        [JsonPropertyName("bestMatchMateTags")]
        public string BestMatchMateTags { get; set; }

        [JsonPropertyName("bestMatchOpponentTags")]
        public string BestMatchOpponentTags { get; set; }

        [JsonPropertyName("bestMatchLastRound")]
        public int BestMatchLastRound { get; set; }

        [JsonPropertyName("bestMatchLastRoundWon")]
        public int BestMatchLastRoundWon { get; set; }

        [JsonPropertyName("bestMatchDeaths")]
        public int BestMatchDeaths { get; set; }

        [JsonPropertyName("bestMatchKills")]
        public int BestMatchKills { get; set; }

        [JsonPropertyName("bestMatchKillDeathRatio")]
        public double BestMatchKillDeathRatio { get; set; }

        [JsonPropertyName("bestMatchRoundsWonList")]
        public string BestMatchRoundsWonList { get; set; }

        [JsonPropertyName("bestMatchBestRound")]
        public int BestMatchBestRound { get; set; }

        [JsonPropertyName("bestMatchBestRoundWon")]
        public int BestMatchBestRoundWon { get; set; }

        [JsonPropertyName("bestMatchBestRoundDeath")]
        public int BestMatchBestRoundDeath { get; set; }

        [JsonPropertyName("bestMatchBestRoundHeadshots")]
        public int BestMatchBestRoundHeadshots { get; set; }

        [JsonPropertyName("bestMatchBestRoundKills")]
        public int BestMatchBestRoundKills { get; set; }

        [JsonPropertyName("bestMatchBestRoundOperator")]
        public string BestMatchBestRoundOperator { get; set; }

        [JsonPropertyName("bestMatchBestRoundTimeAlive")]
        public int BestMatchBestRoundTimeAlive { get; set; }
    }

    public class WeeklyFullPlayerStatistics
    {
        [JsonPropertyName("profileId")]
        public Guid ProfileId { get; set; }

        [JsonPropertyName("year")]
        public int Year { get; set; }

        [JsonPropertyName("week")]
        public int Week { get; set; }

        [JsonPropertyName("gamemode")]
        public string Gamemode { get; set; }

        [JsonPropertyName("daysPlayed")]
        public int DaysPlayed { get; set; }

        [JsonPropertyName("matchesPlayed")]
        public int MatchesPlayed { get; set; }

        [JsonPropertyName("matchesWon")]
        public int MatchesWon { get; set; }

        [JsonPropertyName("matchesLost")]
        public int MatchesLost { get; set; }

        [JsonPropertyName("roundsPlayed")]
        public int RoundsPlayed { get; set; }

        [JsonPropertyName("roundsWon")]
        public int RoundsWon { get; set; }

        [JsonPropertyName("roundsLost")]
        public int RoundsLost { get; set; }

        [JsonPropertyName("playerScore")]
        public int PlayerScore { get; set; }

        [JsonPropertyName("minutesAlive")]
        public int MinutesAlive { get; set; }

        [JsonPropertyName("minutesDead")]
        public int MinutesDead { get; set; }

        [JsonPropertyName("deaths")]
        public int Deaths { get; set; }

        [JsonPropertyName("kills")]
        public int Kills { get; set; }

        [JsonPropertyName("assists")]
        public int Assists { get; set; }

        [JsonPropertyName("headshots")]
        public int Headshots { get; set; }

        [JsonPropertyName("meleeKills")]
        public int MeleeKills { get; set; }

        [JsonPropertyName("teamKills")]
        public int TeamKills { get; set; }

        [JsonPropertyName("trades")]
        public int Trades { get; set; }

        [JsonPropertyName("openingKills")]
        public int OpeningKills { get; set; }

        [JsonPropertyName("openingDeaths")]
        public int OpeningDeaths { get; set; }

        [JsonPropertyName("openingKillTrades")]
        public int OpeningKillTrades { get; set; }

        [JsonPropertyName("openingDeathTrades")]
        public int OpeningDeathTrades { get; set; }

        [JsonPropertyName("revives")]
        public int Revives { get; set; }

        [JsonPropertyName("roundsWithAKill")]
        public int RoundsWithAKill { get; set; }

        [JsonPropertyName("roundsWithMultiKill")]
        public int RoundsWithMultiKill { get; set; }

        [JsonPropertyName("roundsWithAnAce")]
        public int RoundsWithAnAce { get; set; }

        [JsonPropertyName("roundsWithKost")]
        public int RoundsWithKost { get; set; }

        [JsonPropertyName("roundsWithClutch")]
        public int RoundsWithClutch { get; set; }

        [JsonPropertyName("roundsSurvived")]
        public int RoundsSurvived { get; set; }

        [JsonPropertyName("distanceTravelled")]
        public int DistanceTravelled { get; set; }
    }
}