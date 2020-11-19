using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static R6Sharp.Parser;

namespace R6Sharp.Response
{
    public enum StatisticType { Maps, Operators, Summary, Trend, Weapons }

    public class DataResponse
    {
        [JsonPropertyName("profileId")]
        public Guid ProfileId { get; set; }

        [JsonPropertyName("startDate")]
        [JsonConverter(typeof(ParseStringToDateTime))]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        [JsonConverter(typeof(ParseStringToDateTime))]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("region")]
        [JsonConverter(typeof(ParseStringToRegion))]
        public Region Region { get; set; }

        [JsonPropertyName("statType")]
        [JsonConverter(typeof(ParseStringToStatisticType))]
        public StatisticType StatType { get; set; }
    }
}