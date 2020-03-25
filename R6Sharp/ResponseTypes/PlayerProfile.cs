using System;
using System.Text.Json.Serialization;

namespace R6Sharp.ResponseTypes
{
    public class PlayerProfile
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