using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.ResponseTypes
{
    public class ProfileSearch
    {
        [JsonPropertyName("player_profiles")]
        public List<Profile> Profiles { get; set; }
    }

    public class Profile
    {
        [JsonPropertyName("xp")]
        public int XP { get; set; }

        [JsonPropertyName("profile_id")]
        public Guid ProfileId { get; set; }

        [JsonPropertyName("lootbox_probability")]
        public int LootboxProbability { get; set; }

        [JsonPropertyName("level")]
        public int Level { get; set; }
    }
}