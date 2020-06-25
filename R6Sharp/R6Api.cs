using R6Sharp.Response;
using R6Sharp.Response.Statistic;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;

namespace R6Sharp
{
    /// <summary>
    /// Region the player is based in.
    /// </summary>
    public enum Region
    {
        APAC,   // Asia Pacific :(
        EMEA,   // Europe, Middle East and Africa
        NCSA    // North, Central and South America
    }

    /// <summary>
    /// Platform the player is based on.
    /// </summary>
    public enum Platform
    {
        Uplay,  // Uplay
        PSN,    // Playstation Network
        XBL     // Xbox Live
    }

    public class R6Api
    {
        #region Fields
        /// <summary>
        /// Credential in base64 acquired from constructor.
        /// </summary>
        private readonly string _credentialsb64;

        /// <summary>
        /// Control if Ubisoft's session handler should remember current session.
        /// </summary>
        public bool RememberMe { get; set; }

        /// <summary>
        /// Current session details with Ubisoft.
        /// </summary>
        public Session CurrentSession { get; private set; }
        #endregion

        #region Constructors
        /// <summary>
        /// Create a R6S API instance with remember me defaulting to true
        /// </summary>
        /// <param name="email">
        /// Email address of a Ubisoft account.
        /// </param>
        /// <param name="password">
        /// Password of a Ubisoft account.
        /// </param>
        public R6Api(string email, string password) : this(email, password, true)
        {

        }

        /// <summary>
        /// Create a R6S API instance with optional remember me option.
        /// </summary>
        /// <param name="email">
        /// Email address of a Ubisoft account.
        /// </param>
        /// <param name="password">
        /// Password of a Ubisoft account.
        /// </param>
        /// <param name="rememberMe">
        /// Option for Ubisoft to remember this instance's session (can be changed over time by <see cref="RememberMe"/>.
        /// </param>
        public R6Api(string email, string password, bool rememberMe)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(this.GetType().FullName, "Email address cannot be null or empty.");
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(this.GetType().FullName, "Password cannot be null or empty.");
            }

            RememberMe = rememberMe;
            // Generate an auth for acquiring a token
            var auth = $"{email}:{password}";
            var bytes = Encoding.UTF8.GetBytes(auth);
            _credentialsb64 = Convert.ToBase64String(bytes);
        }
        #endregion

        #region Publics
        /// <summary>
        /// Search for a player on Rainbow 6 Siege.
        /// </summary>
        /// <param name="players">
        /// The player names to search for.
        /// </param>
        /// <param name="platform">
        /// The platform the player is on.
        /// </param>
        /// <returns>
        /// A list of players that matched the terms.
        /// </returns>
        public async Task<List<Profile>> SearchProfileAsync(string[] players, Platform platform)
        {
            var queries = HttpUtility.ParseQueryString(string.Empty);
            queries.Add("nameOnPlatform", HttpUtility.UrlEncode(string.Join(',', players)));
            queries.Add("platformType", Constant.PlatformToString(platform));

            var results = await GetDataAsync<ProfileSearch>(Endpoint.Search, null, queries.ToString()).ConfigureAwait(false);
            return results.Profiles;
        }

        /// <summary>
        /// Get a list of basic profiles (like <see cref="PlayerProgression.XP"/> and <see cref="PlayerProgression.Level"/>).
        /// </summary>
        /// <param name="uuids">
        /// The UUIDs matching the player profiles (should be searched with <see cref="SearchProfileAsync(string, Platform)"/> beforehand).
        /// </param>
        /// <param name="platform">
        /// The platform <paramref name="uuids"/> belong to.
        /// </param>
        /// <returns>
        /// A list of basic profiles matching the request terms.
        /// </returns>
        public async Task<List<PlayerProgression>> GetProfileAsync(Guid[] uuids, Platform platform)
        {
            var queries = HttpUtility.ParseQueryString(string.Empty);
            queries.Add("profile_ids", string.Join(',', uuids));

            var results = await GetDataAsync<PlayerProgressionSearch>(Endpoint.Progressions, platform, queries.ToString()).ConfigureAwait(false);
            foreach (var result in results.PlayerProgressions)
            {
                // Attach link to player profile icon url
                //result.ProfileIcon = new Uri($"https://ubisoft-avatars.akamaized.net/{result.ProfileId.ToString()}/default_146_146.png?appId={Constants.Rainbow6S.ToString()}");
                result.ProfileIcon = new Uri(string.Format(Endpoint.Avatar, result.ProfileId, Constant.Rainbow6S));
            }
            return results.PlayerProgressions;
        }

        /// <summary>
        /// Get a list of ranked profiles (like <see cref="Ranked.SkillMean"/> or <see cref="Ranked.MMR"/>).
        /// </summary>
        /// <param name="uuids">
        /// The UUIDs matching the player profiles (should be searched with <see cref="SearchProfileAsync(string, Platform)"/> beforehand).
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
        public async Task<Dictionary<string, Ranked>> GetRankedAsync(Guid[] uuids, Platform platform, Region region, int season)
        {
            var queries = HttpUtility.ParseQueryString(string.Empty);
            queries.Add("profile_ids", string.Join(',', uuids));
            queries.Add("board_id", "pvp_ranked");
            queries.Add("region_id", Constant.RegionToString(region));
            queries.Add("season_id", season.ToString());

            var results = await GetDataAsync<RankedSearch>(Endpoint.Ranked, platform, queries.ToString()).ConfigureAwait(false);
            return results.Players;
        }

        /// <summary>
        /// Get a list of overall profiles (like <see cref="CoreStatistic.GeneralDeaths"/> or <see cref="CoreStatistic.GeneralTimePlayed"/>).
        /// </summary>
        /// <param name="uuids">
        /// The UUIDs matching the player profiles (should be searched with <see cref="SearchProfileAsync(string, Platform)"/> beforehand).
        /// </param>
        /// <param name="platform">
        /// The platform <paramref name="uuids"/> belong to.
        /// </param>
        /// <param name="allStats">
        /// Get all stats from Ubisoft (note: this takes longer than others and contains everything e.g. weapons, operators).
        /// </param>
        /// <returns>
        /// A dictionary of players matching the request terms in a dictionary (to be referenced with the player UUID as key).
        /// </returns>
        public async Task<Dictionary<string, Statistic>> GetStatisticsAsync(Guid[] uuids, Platform platform, bool allStats)
        {
            var results = await FetchStatisticsAsync<Statistics>(uuids, platform, allStats ? Constant.AllStatisticsVariables : Constant.CoreStatisticsVariables).ConfigureAwait(false);
            return results.Stats;
        }

        /// <summary>
        /// Get objective-specific information, such as hostage rescue or secure area, on players.
        /// </summary>
        /// <param name="uuids">
        /// The UUIDs matching the player profiles (should be searched with <see cref="SearchProfileAsync(string, Platform)"/> beforehand).
        /// </param>
        /// <param name="platform">
        /// The platform <paramref name="uuids"/> belong to.
        /// </param>
        /// <returns>
        /// A dictionary of players with players' UUIDs as key and objectives as values.
        /// </returns>
        public async Task<Dictionary<string, ObjectiveStatistic>> GetObjectiveStatisticsAsync(Guid[] uuids, Platform platform)
        {
            var results = await FetchStatisticsAsync<ObjectiveStatisticFetch>(uuids, platform, Constant.ObjectiveStatisticsVariables).ConfigureAwait(false);
            return results.Players;
        }
        #endregion

        #region Overloading Methods
        public async Task<Profile> SearchProfileAsync(string player, Platform platform)
        {
            var profiles = await SearchProfileAsync(new string[] { player }, platform).ConfigureAwait(false);
            // the search result could contain more than one result, return first anyways
            return profiles.Count > 0 ? profiles[0] : null;
        }

        public async Task<PlayerProgression> GetProfileAsync(Guid uuid, Platform platform)
        {
            var profiles = await GetProfileAsync(new[] { uuid }, platform).ConfigureAwait(false);
            return profiles.Count > 0 ? profiles[0] : null;
        }

        public async Task<Dictionary<string, Ranked>> GetRankedAsync(Guid uuid, Platform platform, Region region)
        {
            return await GetRankedAsync(uuid, platform, region, -1).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, Ranked>> GetRankedAsync(Guid[] uuids, Platform platform, Region region)
        {
            return await GetRankedAsync(uuids, platform, region, -1).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, Ranked>> GetRankedAsync(Guid uuid, Platform platform, Region region, int season)
        {
            return await GetRankedAsync(new[] { uuid }, platform, region, season).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, Statistic>> GetStatisticsAsync(Guid uuid, Platform platform, bool allStats)
        {
            return await GetStatisticsAsync(new[] { uuid }, platform, allStats).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, ObjectiveStatistic>> GetObjectiveStatisticsAsync(Guid uuid, Platform platform)
        {
            return await GetObjectiveStatisticsAsync(new[] { uuid }, platform).ConfigureAwait(false);
        }
        #endregion

        #region Privates
        private async Task<T> FetchStatisticsAsync<T>(Guid[] uuids, Platform platform, string variables)
        {
            var queries = HttpUtility.ParseQueryString(string.Empty);
            queries.Add("populations", string.Join(',', uuids));
            queries.Add("statistics", string.Join(',', variables));

            var results = await GetDataAsync<T>(Endpoint.Statistics, platform, queries.ToString()).ConfigureAwait(false);
            return results;
        }

        private async Task<T> GetDataAsync<T>(string url, Platform? platform, string queries)
        {
            if (platform != null)
            {
                url = string.Format(url, Constant.PlatformToGuid(platform ?? default));
            }
            if (queries != null)
            {
                url = $"{url}?{queries}";
            }

            var uri = new Uri(url);
            // Add authorization header with ticket
            var headerValuePairs = new[]
            {
                new KeyValuePair<HttpRequestHeader, string>(HttpRequestHeader.Authorization, $"Ubi_v1 t={await GetTicketAsync().ConfigureAwait(false)}")
            };
            var response = await BuildRequestAsync(uri, headerValuePairs, null, true).ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(response);
        }

        /// <summary>
        /// Gets ticket from Ubisoft through session making. Calling this even if a session exists will
        /// check if session has expired or not and refresh accordingly.
        /// </summary>
        /// <param name="rememberMe">
        /// Control if Ubisoft session handler should remember this session/connection.
        /// </param>
        /// <returns>
        /// The ticket returned from server.
        /// </returns>
        private async Task<string> GetTicketAsync()
        {
            // If a session exist(s/ed), check if it has expired
            bool expired = true;
            if (CurrentSession != null)
            {
                var now = DateTime.UtcNow;
                if (now < CurrentSession.Expiration)
                {
                    expired = false;
                }
            }

            // If any session expired or doesn't exist (by default)
            if (expired)
            {
                // Build json for remembering (or not) the user/session
                byte[] data = Encoding.UTF8.GetBytes($"{{\"rememberMe\": {(RememberMe ? "true" : "false")}}}");
                // Add authorization header
                var headervaluepairs = new []
                {
                    new KeyValuePair<HttpRequestHeader, string>(HttpRequestHeader.Authorization, $"Basic {_credentialsb64}")
                };

                // Get result from endpoint
                var response = await BuildRequestAsync(new Uri(Endpoint.Sessions), headervaluepairs, data, false).ConfigureAwait(false);
                CurrentSession = JsonSerializer.Deserialize<Session>(response);
            }

            return CurrentSession.Ticket;
        }

        private static async Task<string> BuildRequestAsync(Uri uri, KeyValuePair<HttpRequestHeader, string>[] additionalHeaderValues, byte[] data, bool get)
        {
            // Build a web request to endpoint
            var request = WebRequest.CreateHttp(uri);
            // Set request method
            request.Method = get ? WebRequestMethods.Http.Get : WebRequestMethods.Http.Post;
            // Apply usual request headers that should be in all requests to Ubisoft
            request.Headers.Add(HttpRequestHeader.ContentType, MediaTypeNames.Application.Json);
            request.Headers.Add("Ubi-AppId", Constant.Rainbow6S.ToString());
            // Apply auxiliary headers supplied to method
            foreach (var additionalHeaderValue in additionalHeaderValues)
            {
                request.Headers.Add(additionalHeaderValue.Key, additionalHeaderValue.Value);
            }

            // If we have some data to send, write it to stream (make sure it is POST)
            if (data != null && request.Method.Equals("POST"))
            {
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    await stream.WriteAsync(data).ConfigureAwait(false);
                }
            }

            string result;
            // Get result from Ubisoft and grab the json
            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            using (var reader = new StreamReader(stream))
            {
                result = await reader.ReadToEndAsync().ConfigureAwait(false);
            }

            return result;
        }
        #endregion
    }
}
