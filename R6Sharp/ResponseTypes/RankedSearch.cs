using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.ResponseTypes
{
    internal class RankedSearch
    {
        [JsonPropertyName("players")]
        internal Dictionary<string, Ranked> Players { get; set; }
    }
}