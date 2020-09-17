using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.Response.Static
{
    public enum RankId
    {
        Copper,
        Bronze,
        Silver,
        Gold,
        Platinum,
        Diamond,
        Champion
    };

    public class RankFetch
    {
        [JsonPropertyName("seasons")]
        public List<SeasonDetail> Seasons { get; set; }
    }

    public class SeasonDetail
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("divisions")]
        public List<Division> Divisions { get; set; }

        [JsonPropertyName("ranks")]
        public List<Rank> Ranks { get; set; }

        [JsonPropertyName("placementGames")]
        public int PlacementGames { get; set; }
    }

    public class Division
    {
        [JsonPropertyName("id")]
        [JsonConverter(typeof(ApiHelper.ParseStringToId))]
        public RankId Id { get; set; }

        [JsonPropertyName("name")]
        public Name Name { get; set; }

        [JsonPropertyName("ranks")]
        public List<int> Ranks { get; set; }
    }

    public class Name
    {
        [JsonPropertyName("oasisId")]
        public int OasisId { get; set; }
    }

    public class Rank
    {
        [JsonPropertyName("name")]
        public Name Name { get; set; }

        [JsonPropertyName("images")]
        public Images Images { get; set; }

        [JsonPropertyName("range")]
        public List<int?> Range { get; set; }
    }

    public class Images
    {
        [JsonPropertyName("default")]
        public string Default { get; set; }

        [JsonPropertyName("hd")]
        public string Hd { get; set; }
    }
}