using System;
using System.Text.Json.Serialization;
using static R6Sharp.Parser;

namespace R6Sharp.Response.DataResponse
{
    public class SeasonalDataResponse<T> : DataResponse<T>
    {
        [JsonPropertyName("executionTime")]
        public int ExecutionTime { get; set; }

        [JsonPropertyName("returnedTime")]
        [JsonConverter(typeof(ParseStringToDateTime))]
        public DateTime ReturnedTime { get; set; }
    }
}
