using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.Response.Static
{
    public class SeasonsInfo
    {
        [JsonPropertyName("seasons")]
        public Dictionary<string, Season> Seasons { get; set; }

        [JsonPropertyName("latestSeason")]
        [JsonConverter(typeof(ApiHelper.ParseStringToInt))]
        public int LatestSeason { get; set; }
    }

    public class Season
    {
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("background")]
        public Uri Background { get; set; }
    }
}
