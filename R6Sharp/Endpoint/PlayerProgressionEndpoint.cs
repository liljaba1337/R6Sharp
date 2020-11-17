using R6Sharp.Response;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace R6Sharp.Endpoint
{
    public class PlayerProgressionEndpoint
    {
        private readonly SessionEndpoint _sessionHandler;

        public PlayerProgressionEndpoint(SessionEndpoint sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }

        /// <summary>
        /// Get a list of basic profiles (like <see cref="PlayerProgression.XP"/> and <see cref="PlayerProgression.Level"/>).
        /// </summary>
        /// <param name="uuids">
        /// The UUIDs matching the player profiles (should be searched with <see cref="GetProfileAsync(string, Platform)"/> beforehand).
        /// </param>
        /// <param name="platform">
        /// The platform <paramref name="uuids"/> belong to.
        /// </param>
        /// <returns>
        /// A list of basic profiles matching the request terms.
        /// </returns>
        public async Task<List<PlayerProgression>> GetPlayerProgressionAsync(Guid[] uuids, Platform platform)
        {
            var queries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("profile_ids", string.Join(',', uuids))
            };

            var ticket = await _sessionHandler.GetTicketAsync().ConfigureAwait(false);
            var results = await ApiHelper.GetDataAsync(Endpoints.Progressions, platform, queries, ticket).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<PlayerProgressionFetch>(results);
            foreach (var result in deserialised.PlayerProgressions)
            {
                // Attach link to player profile icon url
                result.ProfileIcon = new Uri(string.Format(Endpoints.Avatar, result.ProfileId, Constant.Rainbow6S));
            }
            return deserialised.PlayerProgressions;
        }

        /// <inheritdoc/>
        public async Task<PlayerProgression> GetPlayerProgressionAsync(Guid uuid, Platform platform)
        {
            var profiles = await GetPlayerProgressionAsync(new[] { uuid }, platform).ConfigureAwait(false);
            return profiles.Count > 0 ? profiles[0] : null;
        }
    }
}
