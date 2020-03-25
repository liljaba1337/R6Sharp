using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.ResponseTypes
{
    internal class PlayerProfileSearch
    {
        [JsonPropertyName("player_profiles")]
        internal List<PlayerProfile> Profiles { get; set; }
    }
}
