using System;
using System.Text.Json.Serialization;
using static R6Sharp.Parser;

namespace R6Sharp.Response.DataResponse
{
    public class CurrentDataResponse<T> : DataResponse<T>
    {
        [JsonPropertyName("startDate")]
        [JsonConverter(typeof(ParseStringToDateTime))]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("endDate")]
        [JsonConverter(typeof(ParseStringToDateTime))]
        public DateTime EndDate { get; set; }
    }
}