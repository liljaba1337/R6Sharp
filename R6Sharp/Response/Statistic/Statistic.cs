using System;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.Response.Statistic
{
    public class Statistics
    {
        [JsonPropertyName("results")]
        public Dictionary<string, Statistic> Stats { get; set; }
    }


    public class Statistic : CoreStatistic
    {
        // TO-DO add the rest of the stats (weapon/operator)
    }
}
