using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.ResponseTypes
{
    public class AllStatisticsFetch
    {
        [JsonPropertyName("results")]
        public Dictionary<string, CoreStatistics> All { get; set; }
    }


    public class AllStatistics : CoreStatistics
    {
    }
}
