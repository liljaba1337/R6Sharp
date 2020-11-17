using R6Sharp.Response;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace R6Sharp.Endpoint
{
    public class PlayerEndpoint
    {
        private readonly SessionEndpoint _sessionHandler;

        public PlayerEndpoint(SessionEndpoint sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }

        /// <summary>
        /// Get a list of ranked profiles (like <see cref="BoardInfo.SkillMean"/> or <see cref="BoardInfo.MMR"/>).
        /// </summary>
        /// <param name="uuids">
        /// The UUIDs matching the player profiles./> beforehand).
        /// </param>
        /// <param name="platform">
        /// The platform <paramref name="uuids"/> belong to.
        /// </param>
        /// <param name="region">
        /// The region <paramref name="uuids"/> belong to.
        /// </param>
        /// <param name="season">
        /// The seasonal stats to search for.
        /// </param>
        /// <returns>
        /// A list of players matching the request terms in a dictionary (to be referenced with the player UUID as key).
        /// </returns>
        public async Task<Dictionary<string, BoardInfo>> GetRankedAsync(Guid[] uuids, Platform platform, Region region, int season)
        {
            var queries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("profile_ids", string.Join(',', uuids)),
                new KeyValuePair<string, string>("board_id", "pvp_ranked"),
                new KeyValuePair<string, string>("region_id", Constant.RegionToString(region)),
                new KeyValuePair<string, string>("season_id", season.ToString())
            };

            var ticket = await _sessionHandler.GetTicketAsync().ConfigureAwait(false);
            var results = await ApiHelper.GetDataAsync(Endpoints.Players, platform, queries, ticket).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<BoardInfoFetch>(results);
            return deserialised.Players;
        }

        /// <summary>
        /// Get a list of casual profiles (like <see cref="BoardInfo.SkillMean"/> or <see cref="BoardInfo.MMR"/>).
        /// </summary>
        /// <param name="uuids">
        /// The UUIDs matching the player profiles./> beforehand).
        /// </param>
        /// <param name="platform">
        /// The platform <paramref name="uuids"/> belong to.
        /// </param>
        /// <param name="region">
        /// The region <paramref name="uuids"/> belong to.
        /// </param>
        /// <param name="season">
        /// The seasonal stats to search for.
        /// </param>
        /// <returns>
        /// A list of players matching the request terms in a dictionary (to be referenced with the player UUID as key).
        /// </returns>
        public async Task<Dictionary<string, BoardInfo>> GetCasualAsync(Guid[] uuids, Platform platform, Region region, int season)
        {
            var queries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("profile_ids", string.Join(',', uuids)),
                new KeyValuePair<string, string>("board_id", "pvp_casual"),
                new KeyValuePair<string, string>("region_id", Constant.RegionToString(region)),
                new KeyValuePair<string, string>("season_id", season.ToString())
            };

            var ticket = await _sessionHandler.GetTicketAsync().ConfigureAwait(false);
            var results = await ApiHelper.GetDataAsync(Endpoints.Players, platform, queries, ticket).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<BoardInfoFetch>(results);
            return deserialised.Players;
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, BoardInfo>> GetRankedAsync(Guid uuid, Platform platform, Region region)
        {
            return await GetRankedAsync(uuid, platform, region, -1).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, BoardInfo>> GetRankedAsync(Guid[] uuids, Platform platform, Region region)
        {
            return await GetRankedAsync(uuids, platform, region, -1).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, BoardInfo>> GetRankedAsync(Guid uuid, Platform platform, Region region, int season)
        {
            return await GetRankedAsync(new[] { uuid }, platform, region, season).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, BoardInfo>> GetCasualAsync(Guid uuid, Platform platform, Region region)
        {
            return await GetCasualAsync(uuid, platform, region, -1).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, BoardInfo>> GetCasualAsync(Guid[] uuids, Platform platform, Region region)
        {
            return await GetCasualAsync(uuids, platform, region, -1).ConfigureAwait(false);
        }

        /// <inheritdoc/>
        public async Task<Dictionary<string, BoardInfo>> GetCasualAsync(Guid uuid, Platform platform, Region region, int season)
        {
            return await GetCasualAsync(new[] { uuid }, platform, region, season).ConfigureAwait(false);
        }
    }
}
