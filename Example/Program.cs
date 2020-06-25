using R6Sharp;
using System;
using System.IO;
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

            var search = api.SearchProfileAsync("GarlicSos", Platform.Uplay).Result;
            // var profile = api.GetProfileAsync(guids, R6Api.Platform.Uplay).Result;
            var profile = api.GetProfileAsync(search.ProfileId, Platform.Uplay).Result;
            var ranked = api.GetRankedAsync(search.ProfileId, Platform.Uplay, Region.EMEA).Result;
            var core = api.GetStatisticsAsync(search.ProfileId, Platform.Uplay, false).Result;
            var objectives = api.GetObjectiveStatisticsAsync(search.ProfileId, Platform.Uplay).Result;

            Console.WriteLine($"Profile ID:         {search.UserId}");
            Console.WriteLine($"Level:              {profile.Level}");
            Console.WriteLine($"Rank:               {ranked[profile.ProfileId.ToString()].Rank}");
            Console.WriteLine($"Ranked Matches:     {core[profile.ProfileId.ToString()].RankedMatchesPlayed}");
            Console.WriteLine($"Best Bomb Score:    {objectives[profile.ProfileId.ToString()].BombBestScore}");
        }
    }
}