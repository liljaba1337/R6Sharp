using R6Sharp;
using R6Sharp.Response;
using System;
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

            var username = "Pseudosin";
            var platform = Platform.PC;

            Profile profile = api.Profile.GetProfileAsync(username, platform).Result;
            var uuid = profile.UserId;

            PlayerProgression playerProgression = api.PlayerProgression.GetPlayerProgressionAsync(uuid, platform).Result;
            var playerLevel = playerProgression.Level;

            // As of season 18, regional board (ranked and casual) statistics have merged and cross regions now
            BoardInfo playerRanked = api.Player.GetRankedAsync(uuid, platform).Result;
            var playerMMR = playerRanked.MMR;
            // get casual queue stats
            // var playerCasual = api.Player.GetCasualAsync(uuid, platform);                            
            // get season 17 ranked stats for EMEA
            // var playerRankedSeason17 = api.Player.GetRankedAsync(uuid, platform, Region.EMEA, 17);

            var seasons = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19 };
            PlayersSkillRecords playerSkillRecords = api.PlayersSkillRecordsEndpoint
                                                        .GetPlayerSkillRecordsAsync(uuid, platform, Region.EMEA, seasons).Result;
            var recordMMR = playerSkillRecords.SeasonsPlayerSkillRecords[^1]
                                              .RegionsPlayerSkillRecords[0]
                                              .BoardsPlayerSkillRecords[0]
                                              .PlayerSkillRecords[0]
                                              .MMR;

            var gamemodes = Gamemode.All | Gamemode.Ranked | Gamemode.Unranked | Gamemode.Casual;
            var from = new DateTime(2020, 06, 16);
            var to = new DateTime(2020, 11, 26);

            var summary = api.GetSummaryAsync(uuid, gamemodes, platform, from, to).Result;
            var operators = api.GetOperatorAsync(uuid, gamemodes, platform, TeamRole.Attacker | TeamRole.Defender, from, to).Result;
            var maps = api.GetMapAsync(uuid, gamemodes, platform, TeamRole.All | TeamRole.Attacker | TeamRole.Defender, from, to).Result;
            var weapons = api.GetWeaponAsync(uuid, gamemodes, platform, TeamRole.All, from, to).Result;
            var trends = api.GetTrendAsync(uuid, gamemodes, from, to, TeamRole.All | TeamRole.Attacker | TeamRole.Defender, TrendType.Weeks).Result;
            var seasonal = api.GetSeasonalAsync(uuid, gamemodes, platform).Result;
            var narrative = api.GetNarrativeAsync(uuid, from, to).Result;
            Console.WriteLine(uuid);
        }
    }
}