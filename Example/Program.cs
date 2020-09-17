using R6Sharp;
using R6Sharp.Endpoint;
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
            var json = File.ReadAllText(@"credentials.json");
            var credentials = JsonSerializer.Deserialize<Credential>(json);
            var api = new R6Api(credentials.Email, credentials.Password);

            var guids = new[]
            {
                Guid.Parse("00000000-0000-0000-0000-000000000000"),
                Guid.Parse("11111111-1111-1111-1111-111111111111"),
                Guid.Parse("22222222-2222-2222-2222-222222222222"),
                Guid.Parse("33333333-3333-3333-3333-333333333333"),
                Guid.Parse("44444444-4444-4444-4444-444444444444")
            };

            #region Player Data
            var username = "Pseudosin";
            var platform = Platform.Uplay;
            var region = Region.EMEA;

            Profile profile = api.Profile.GetProfileAsync(username, platform).Result;
            // var profile = api.GetProfileAsync(guids, platform).Result;
            Console.WriteLine($"Profile ID:         {profile.UserId}");

            PlayerProgression progression = api.PlayerProgression.GetPlayerProgressionAsync(profile.ProfileId, platform).Result;
            Console.WriteLine($"Level:              {progression.Level}");

            Dictionary<string, BoardInfo> ranked = api.Player.GetRankedAsync(profile.ProfileId, platform, region).Result;
            Console.WriteLine($"Ranked Rank:        {ranked[progression.ProfileId.ToString()].Rank}");

            Dictionary<string, BoardInfo> casual = api.Player.GetCasualAsync(profile.ProfileId, platform, region).Result;
            Console.WriteLine($"Casual Rank:        {casual[progression.ProfileId.ToString()].Rank}");

            EquipmentStatistic equipments = api.Statistic.GetEquipmentStatistics(profile.ProfileId, Platform.Uplay).Result;
            GamemodeStatistic gamemodes = api.Statistic.GetGamemodeStatistics(profile.ProfileId, Platform.Uplay).Result;
            OperatorStatistic operators = api.Statistic.GetOperatorStatistics(profile.ProfileId, Platform.Uplay).Result;
            QueueStatistic queues = api.Statistic.GetQueueStatistics(profile.ProfileId, Platform.Uplay).Result;
            TerroristHuntMissionStatistic terroristhuntmissions = api.Statistic.GetTerroristHuntMissionsStatistics(profile.ProfileId, Platform.Uplay).Result;
            #endregion

            #region Static Data
            SeasonsInfo seasonsInfo = api.Static.GetSeasonsInfoAsync().Result;
            Console.WriteLine($"Latest Season:      {seasonsInfo.LatestSeason}");

            Season season = api.Static.GetSeasonAsync(seasonsInfo.LatestSeason).Result;
            // Season season = Season.GetSeasonAsync().Result;
            Console.WriteLine($"Current Season:     {season.Id}");

            Dictionary<string, string> locales = api.Static.GetLocaleAsync(Language.BritishEnglish).Result;
            List<SeasonDetail> seasonDetails = api.Static.GetSeasonDetailsAsync().Result;

            var seasonId = 18;
            // Find season details for season 18
            var seasonDetail = seasonDetails.Find(x => x.Id == seasonId);
            // Get the highest rank for that season (should be champion)
            var highestSeasonRank = seasonDetail.Ranks[^1];
            var rankOasis = highestSeasonRank.Name.OasisId;
            // Get rank name from locale dictionary to localise variable names (e.g. champion -> şampiyon)
            Console.WriteLine($"Highest Rank Name:  {locales[rankOasis.ToString()]}");
            Console.WriteLine($"Highest Rank URL:   {highestSeasonRank.Images.Hd}");
            #endregion
        }
    }
}