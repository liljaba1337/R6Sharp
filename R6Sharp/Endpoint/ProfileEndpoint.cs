using R6Sharp.Response;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace R6Sharp.Endpoint
{
    public class ProfileEndpoint
    {
        private readonly SessionEndpoint _sessionHandler;

        public ProfileEndpoint(SessionEndpoint sessionHandler)
        {
            _sessionHandler = sessionHandler;
        }

        /// <summary>
        /// Search for a player on Rainbow 6 Siege.
        /// </summary>
        /// <param name="players">
        /// The player names to search for.
        /// </param>
        /// <param name="platform">
        /// The platform the player is on.d
        /// </param>
        /// <returns>
        /// A list of players that matched the terms.
        /// </returns>
        public async Task<List<Profile>> GetProfileAsync(string[] players, Platform platform)
        {
            var queries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("nameOnPlatform", HttpUtility.UrlEncode(string.Join(',', players))),
                new KeyValuePair<string, string>("platformType", Constant.PlatformToString(platform))
            };

            var ticket = await _sessionHandler.GetTicketAsync().ConfigureAwait(false);
            var results = await ApiHelper.GetDataAsync(Endpoints.Search, null, queries, ticket).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<ProfileSearch>(results);
            return deserialised.Profiles;
        }

        /// <inheritdoc/>
        public async Task<Profile> GetProfileAsync(string player, Platform platform)
        {
            var profiles = await GetProfileAsync(new string[] { player }, platform).ConfigureAwait(false);
            // the search result could contain more than one result, return first anyways
            return profiles.Count > 0 ? profiles[0] : null;
        }

        public async Task<List<Profile>> GetProfileAsync(Guid[] uuids, Platform platform)
        {
            var queries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("idOnPlatform", HttpUtility.UrlEncode(string.Join(',', uuids))),
                new KeyValuePair<string, string>("platformType", Constant.PlatformToString(platform))
            };

            var ticket = await _sessionHandler.GetTicketAsync().ConfigureAwait(false);
            var results = await ApiHelper.GetDataAsync(Endpoints.Search, null, queries, ticket).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<ProfileSearch>(results);
            return deserialised.Profiles;
        }

        /// <inheritdoc/>
        public async Task<Profile> GetProfileAsync(Guid uuid, Platform platform)
        {
            var profiles = await GetProfileAsync(new Guid[] { uuid }, platform).ConfigureAwait(false);
            // the search result could contain more than one result, return first anyways
            return profiles.Count > 0 ? profiles[0] : null;
        }
    }
}
