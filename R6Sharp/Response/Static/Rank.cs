using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

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

        /// <summary>
        /// Get overall seasons data (season 2 and onwards).
        /// </summary>
        /// <returns>
        /// List of seasons with their respective details like ranks and divisions.
        /// </returns>
        public static async Task<List<SeasonDetail>> GetAllAsync()
        {
            // Fetch ranked static data
            var fetch = await ApiHelper.GetDataAsync<RankFetch>(Endpoint.Ranks, null, null, null).ConfigureAwait(false);
            // Prefix R6 base URL to the images URL so it is ready to use as it is
            foreach (var season in fetch.Seasons)
            {
                foreach (var rank in season.Ranks)
                {
                    rank.Images.Default = string.Join('/', Endpoint.Base, rank.Images.Default);
                    rank.Images.Hd = string.Join('/', Endpoint.Base, rank.Images.Hd);
                }
            }    
            return fetch.Seasons;
        }
    }

    public class Images
    {
        [JsonPropertyName("default")]
        public string Default { get; set; }

        [JsonPropertyName("hd")]
        public string Hd { get; set; }
    }
}