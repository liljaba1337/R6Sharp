using R6Sharp.Response.Statistic;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace R6Sharp.Response
{
    public class Seasonal : PlayerStatistics
    {
        [JsonPropertyName("seasonYear")]
        public string SeasonYear { get; set; }

        [JsonPropertyName("seasonNumber")]
        public string SeasonNumber { get; set; }
    }
}
