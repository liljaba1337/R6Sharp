using R6Sharp.Response;
using R6Sharp.Response.Statistic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace R6Sharp.Endpoint
{
    public class PlayerStatisticsEndpoint
    {
        private readonly SessionEndpoint _sessionHandler;

        public PlayerStatisticsEndpoint(SessionEndpoint sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }

        public async Task<Dictionary<string, double>> GetQueueStatisticsAsync(Guid uuid, Platform platform)
        {
            var results = await GetQueueStatisticsAsync(new Guid[] { uuid }, platform).ConfigureAwait(false);
            return results[uuid.ToString()];
        }

        public async Task<Dictionary<string, Dictionary<string, double>>> GetQueueStatisticsAsync(Guid[] uuids, Platform platform)
        {
            var queries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("populations", HttpUtility.UrlEncode(string.Join(',', uuids))),
                new KeyValuePair<string, string>("statistics", Constant.QueuesStatisticsVariables)
            };

            var session = await _sessionHandler.GetCurrentSessionAsync().ConfigureAwait(false);
            using var results = await ApiHelper.GetDataAsync(Endpoints.UbiServices.Statistics, platform, queries, session)
                                               .ConfigureAwait(false);
            var deserialised = await JsonSerializer.DeserializeAsync<QueueStatistics>(results)
                                                   .ConfigureAwait(false);
            return deserialised.PlayerQueueStatistics;
        }
    }
}