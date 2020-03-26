using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.ResponseTypes
{
    public class StatisticsFetch
    {
        [JsonPropertyName("results")]
        public Dictionary<string, Statistics> Stats { get; set; }
    }


    public class Statistics : CoreStatistics
    {
        // TO-DO add the rest of the stats (weapon/operator)
    }
}
