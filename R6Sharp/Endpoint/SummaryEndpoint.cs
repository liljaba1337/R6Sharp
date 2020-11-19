using R6Sharp.Response;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace R6Sharp.Endpoint
{
    public class SummaryEndpoint
    {
        private readonly SessionEndpoint _sessionHandler;

        public SummaryEndpoint(SessionEndpoint sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }

        public async Task<DataResponse> GetSummaryAsync(Guid uuid, Gamemode gamemodes, Platform platforms, DateTime start, DateTime end)
        {
            var queries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("gameMode", ApiHelper.DeriveGamemodeFlags(gamemodes)),
                new KeyValuePair<string, string>("platform", ApiHelper.DerivePlatformFlags(platforms)),
                new KeyValuePair<string, string>("startDate", start.ToString("yyyyMMdd")),
                new KeyValuePair<string, string>("endDate", end.ToString("yyyyMMdd"))
            };

            var session = await _sessionHandler.GetCurrentSessionAsync().ConfigureAwait(false);
            var results = await ApiHelper.GetDataAsync(UbiconnectEndpoint.Summary, uuid, queries, session).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<DataResponse>(results);
            return deserialised;
        }
    }
}
