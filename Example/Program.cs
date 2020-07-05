using R6Sharp;
using R6Sharp.Response;
using R6Sharp.Response.Static;
using R6Sharp.Response.Statistic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Example
{
    public static class Program
    {
        internal class Credential
        {
            [JsonPropertyName("email")]
            public string Email { get; set; }

            [JsonPropertyName("password")]
            public string Password { get; set; }
        }

        public static void Main()
        {
            Console.OutputEncoding = Encoding.UTF8;
            // credentials.json = {"email": "email@email.com", "password": "somepassword"}
            var json = File.ReadAllText(@"..\..\..\..\credentials.json");
            var credentials = JsonSerializer.Deserialize<Credential>(json);
            var api = new R6Api(credentials.Email, credentials.Password);

            var guids = new []
            {
                Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Guid.Parse("44444444-4444-4444-4444-444444444444")
            };

            // Player data
            Profile search = api.GetProfileAsync("Pseudosin", Platform.Uplay).Result;
            // var profile = api.GetProfileAsync(guids, R6Api.Platform.Uplay).Result;
            PlayerProgression progression = api.GetProgressionAsync(search.ProfileId, Platform.Uplay).Result;
            Dictionary<string, Ranked> ranked = api.GetRankedAsync(search.ProfileId, Platform.Uplay, Region.EMEA).Result;
            Dictionary<string, Statistic> core = api.GetStatisticsAsync(search.ProfileId, Platform.Uplay, false).Result;
            Dictionary<string, Objective> objectives = api.GetObjectiveStatisticsAsync(search.ProfileId, Platform.Uplay).Result;

            // Static data
            SeasonInfo info = SeasonInfo.GetAllAsync().Result;
            Season season = Season.GetSeasonAsync(info.LatestSeason).Result;
            // Season season = Season.GetSeasonAsync().Result;
            Dictionary<string, string> locales = Locale.GetAllAsync(Locale.Language.BritishEnglish).Result;
            List<SeasonDetail> ranks = Rank.GetAllAsync().Result;

            Console.WriteLine($"Profile ID:         {search.UserId}");
            Console.WriteLine($"Level:              {progression.Level}");
            Console.WriteLine($"Rank:               {ranked[progression.ProfileId.ToString()].Rank}");
            Console.WriteLine($"Ranked Matches:     {core[progression.ProfileId.ToString()].RankedMatchesPlayed}");
            Console.WriteLine($"Best Bomb Score:    {objectives[progression.ProfileId.ToString()].BombBestScore}");
            Console.WriteLine($"Latest Season:      {info.LatestSeason}");
            Console.WriteLine($"Current Season:     {season.Id}");

            var seasonId = 18;
            // Find season details for season 18
            var seasonDetail = ranks.Find(x => x.Id == seasonId);
            // Get the highest rank for that season (should be champion)
            var highestSeasonRank = seasonDetail.Ranks[^1];
            var rankOasis = highestSeasonRank.Name.OasisId;
            // Get rank name from locale dictionary to localise variable names (e.g. champion -> şampiyon)
            Console.WriteLine($"Highest Rank Name:  {locales[rankOasis.ToString()]}");
            Console.WriteLine($"Highest Rank URL:   {highestSeasonRank.Images.Hd}");
        }
    }
}