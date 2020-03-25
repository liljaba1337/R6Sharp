using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.ResponseTypes
{
    internal class PlayerSearch
    {
        [JsonPropertyName("profiles")]
        internal List<Player> Profiles { get; set; }
    }
}
