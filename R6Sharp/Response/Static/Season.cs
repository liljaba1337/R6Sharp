using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace R6Sharp.Response.Static
{
    public class SeasonInfo
    {
        [JsonPropertyName("seasons")]
        public Dictionary<string, Season> Seasons { get; set; }

        [JsonPropertyName("latestSeason")]
        [JsonConverter(typeof(ApiHelper.ParseStringToInt))]
        public int LatestSeason { get; set; }

        /// <summary>
        /// Get information based on past and current seasons.
        /// </summary>
        /// <returns>
        /// Latest season and dictionary of seasons with their respective names and URL to season background.
        /// </returns>
        public static async Task<SeasonInfo> GetAllAsync()
        {
            return await ApiHelper.GetDataAsync<SeasonInfo>(Endpoint.Seasons, null, null, null).ConfigureAwait(false);
        }
    }

    public class Season
    {
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("background")]
        public Uri Background { get; set; }

        /// <summary>
        /// Get information specific to a season.
        /// </summary>
        /// <param name="id">
        /// The season number, such as 18 for Steel Wave, or -1 for latest.
        /// </param>
        /// <returns>
        /// Season details.
        /// </returns>
        public static async Task<Season> GetSeasonAsync(int id)
        {
            var info = await SeasonInfo.GetAllAsync().ConfigureAwait(false);
            var season = info.Seasons[id < 0 ? info.LatestSeason.ToString() : id.ToString()];
            season.Id = id < 0 ? info.LatestSeason : id;
            return season;
        }

        /// <inheritdoc/>
        public static async Task<Season> GetSeasonAsync()
        {
            return await GetSeasonAsync(-1).ConfigureAwait(false);
        }
    }
}
