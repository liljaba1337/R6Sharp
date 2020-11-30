# R6Sharp [![Build status](https://ci.appveyor.com/api/projects/status/ydmu8g5r0dxifa0h?svg=true)](https://ci.appveyor.com/project/SergeantSerk/r6sharp) [![Codacy Badge](https://api.codacy.com/project/badge/Grade/02ef54cb799443bab57ebb543fa78778)](https://app.codacy.com/manual/SergeantSerk/R6Sharp?utm_source=github.com&utm_medium=referral&utm_content=SergeantSerk/R6Sharp&utm_campaign=Badge_Grade_Dashboard) [![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FSergeantSerk%2FR6Sharp.svg?type=shield)](https://app.fossa.io/projects/git%2Bgithub.com%2FSergeantSerk%2FR6Sharp?ref=badge_shield)
## Description
An (unofficial) API wrapper for Rainbow 6 Siege to get player stats from Ubisoft's website. Although Ubisoft does not provide an official API endpoint for developers to retrieve player data (therefore not explicitly sanctioned), their own pages retrieve it through web requests using JavaScript to specific endpoints.
## Usage
Please see the [Example](Example/Program.cs) project to see how to use this wrapper. Alternatively, here's an example:
```csharp
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
                                                    .GetPlayerSkillRecordsAsync(uuid, platform, Region.EMEA, seasons)
                                                    .Result;
        var recordMMR = playerSkillRecords.SeasonsPlayerSkillRecords.Last()
                                          .RegionsPlayerSkillRecords.First()
                                          .BoardsPlayerSkillRecords.First()
                                          .PlayerSkillRecords.First()
                                          .MMR;

        var gamemodes = Gamemode.All | Gamemode.Ranked | Gamemode.Unranked | Gamemode.Casual;
        var from = new DateTime(2020, 06, 16);
        var to = new DateTime(2020, 11, 26);

        var summary = api.GetSummaryAsync(uuid, gamemodes, platform, from, to).Result;
        var summaryKills = summary.Platforms["PC"]
                                  .Gamemodes["all"]
                                  .TeamRoles["all"].Last()
                                  .Kills;

        var operators = api.GetOperatorAsync(uuid, gamemodes, platform, TeamRole.Attacker | TeamRole.Defender, from, to).Result;
        var zofiaRankedWins = operators.Platforms["PC"]
                                          .Gamemodes["ranked"]
                                          .TeamRoles["attacker"]
                                          .Where(x => x.StatsDetail == "Zofia").First()
                                          .RoundsWon;

        var maps = api.GetMapAsync(uuid, gamemodes, platform, TeamRole.All | TeamRole.Attacker | TeamRole.Defender, from, to).Result;
        var kanalDefenderTeamKills = maps.Platforms["PC"]
                                         .Gamemodes["casual"]
                                         .TeamRoles["defender"]
                                         .Where(m => m.StatsDetail == "KANAL").First()
                                         .TeamKills;

        var weapons = api.GetWeaponAsync(uuid, gamemodes, platform, TeamRole.All, from, to).Result;
        var allSpear308HeadshotAccuracy = weapons.Platforms["PC"]
                                                 .Gamemodes["all"]
                                                 .TeamRoles["all"]
                                                 .WeaponSlots
                                                 .PrimaryWeapons
                                                 .WeaponTypes
                                                 .Where(t => t.WeaponTypeType == "Assault Rifles").First()
                                                 .Weapons
                                                 .Where(w => w.WeaponName == "SPEAR .308").First()
                                                 .HeadshotAccuracy;

        var trends = api.GetTrendAsync(uuid, gamemodes, from, to, TeamRole.All | TeamRole.Attacker | TeamRole.Defender, TrendType.Weeks).Result;
        var rankedAttackKDRTrend = trends.Platforms["PC"]
                                         .Gamemodes["ranked"]
                                         .TeamRoles["attacker"].Last()
                                         .KillDeathRatio;

        var seasonal = api.GetSeasonalAsync(uuid, gamemodes, platform).Result;
        var rankedY4S4MinutesPlayed = seasonal.Platforms["PC"]
                                              .Gamemodes["ranked"]
                                              .TeamRoles["all"]
                                              .Where(s => s.SeasonYear == "Y4")
                                              .Where(s => s.SeasonNumber == "S4").First()
                                              .MinutesPlayed;

        var narrative = api.GetNarrativeAsync(uuid, from, to).Result;
        var bestMatchScoreAnyWeek = narrative.Profiles[uuid.ToString()]
                                             .Years.First()
                                             .Value.Weeks.First()
                                             .Value.BestMatchFullStatistics
                                             .Score;
    }
}
```
## Credits
Wrapper powered by Ubisoft Entertainment. All data belongs to Ubisoft and should be attributed to them unless notified otherwise by Ubisoft.
## License
[![FOSSA Status](https://app.fossa.io/api/projects/git%2Bgithub.com%2FSergeantSerk%2FR6Sharp.svg?type=large)](https://app.fossa.io/projects/git%2Bgithub.com%2FSergeantSerk%2FR6Sharp?ref=badge_large)
