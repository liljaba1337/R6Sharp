using R6Sharp.Response;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace R6Sharp.Endpoint
{
    public class PlayersSkillRecordsEndpoint
    {
        private readonly SessionEndpoint _sessionHandler;

        public PlayersSkillRecordsEndpoint(SessionEndpoint sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }

        public async Task<PlayersSkillRecords> GetPlayersSkillRecordsAsync(Guid[] uuids, Platform platform, Region region, params int[] seasons)
        {
            var queries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("profile_ids", string.Join(',', uuids)),
                new KeyValuePair<string, string>("board_ids", "pvp_ranked"),
                new KeyValuePair<string, string>("region_ids", Constant.RegionToString(region)),
                new KeyValuePair<string, string>("season_ids", string.Join(',', seasons))
            };

            var session = await _sessionHandler.GetCurrentSessionAsync().ConfigureAwait(false);
            using var results = await ApiHelper.GetDataAsync(Endpoints.UbiServices.PlayerSkillRecords, platform, queries, session).ConfigureAwait(false);
            var deserialised = await JsonSerializer.DeserializeAsync<PlayersSkillRecords>(results).ConfigureAwait(false);
            return deserialised;
        }

        public async Task<PlayersSkillRecords> GetPlayerSkillRecordsAsync(Guid uuid, Platform platform, Region region, params int[] seasons)
        {
            return await GetPlayersSkillRecordsAsync(new[] { uuid }, platform, region, seasons).ConfigureAwait(false);
        }
    }
}