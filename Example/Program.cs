using R6Sharp;
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

            var gamemodes = Gamemode.Casual | Gamemode.Unranked | Gamemode.Ranked | Gamemode.All;

            var profile = api.Profile.GetProfileAsync(username, platform).Result;
            var data = api.Summary.GetSummaryAsync(profile.UserId, gamemodes, platform, new DateTime(2020, 11, 15), new DateTime(2020, 11, 15)).Result;
            Console.WriteLine(data.ProfileId);
        }
    }
}