using R6Sharp.ResponseTypes;
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
    public class R6Api
    {
        public enum Region
        {
            APAC,   // Asia Pacific :(
            EMEA,   // Europe, Middle East and Africa
            NCSA    // North, Central and South America
        }

        public enum Platform
        {
            Uplay,  // uplay
            PSN,    // psn
            XBL     // xbl
        }

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

        public R6Api(string Email, string Password) : this(Email, Password, true)
        {

        }

        public R6Api(string Email, string Password, bool RememberMe)
        {
            if (string.IsNullOrWhiteSpace(Email))
            {
                throw new ArgumentNullException(this.GetType().FullName, "Email address cannot be null or empty.");
            }
            else if (string.IsNullOrWhiteSpace(Password))
            {
                throw new ArgumentNullException(this.GetType().FullName, "Password cannot be null or empty.");
            }

            this.RememberMe = RememberMe;
            // Generate an auth for acquiring a token
            var auth = $"{Email}:{Password}";
            var bytes = Encoding.UTF8.GetBytes(auth);
            _credentialsb64 = Convert.ToBase64String(bytes);
        }

        public async Task<List<Player>> SearchProfileAsync(string Player, Platform Platform)
        {
            var queries = HttpUtility.ParseQueryString(string.Empty);
            queries.Add("nameOnPlatform", HttpUtility.UrlEncode(Player));
            queries.Add("platformType", PlatformToString(Platform));

            var results = await GetDataAsync<PlayerSearch>(Endpoints.Search, null, queries.ToString()).ConfigureAwait(false);
            return results.Players;
        }

        public async Task<List<Profile>> GetProfileAsync(Guid Uuid, Platform Platform)
        {
            return await GetProfileAsync(new[] { Uuid }, Platform).ConfigureAwait(false);
        }

        public async Task<List<Profile>> GetProfileAsync(Guid[] Uuids, Platform Platform)
        {
            var queries = HttpUtility.ParseQueryString(string.Empty);
            queries.Add("profile_ids", string.Join(',', Uuids));

            var results = await GetDataAsync<ProfileSearch>(Endpoints.Progression, Platform, queries.ToString()).ConfigureAwait(false);
            return results.Profiles;
        }

        public async Task<Dictionary<string, Ranked>> GetRankedAsync(Guid Uuid, Platform Platform, Region Region)
        {
            return await GetRankedAsync(Uuid, Platform, Region, -1).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, Ranked>> GetRankedAsync(Guid Uuid, Platform Platform, Region Region, int Season)
        {
            return await GetRankedAsync(new[] { Uuid }, Platform, Region, Season).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, Ranked>> GetRankedAsync(Guid[] Uuids, Platform Platform, Region Region)
        {
            return await GetRankedAsync(Uuids, Platform, Region, -1).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, Ranked>> GetRankedAsync(Guid[] Uuids, Platform Platform, Region Region, int Season)
        {
            var queries = HttpUtility.ParseQueryString(string.Empty);
            queries.Add("profile_ids", string.Join(',', Uuids));
            queries.Add("board_id", "pvp_ranked");
            queries.Add("region_id", RegionToString(Region));
            queries.Add("season_id", Season.ToString());

            var results = await GetDataAsync<RankedSearch>(Endpoints.Ranked, Platform, queries.ToString()).ConfigureAwait(false);
            return results.Players;
        }

        public async Task<Dictionary<string, Statistics>> GetOverallAsync(Guid Uuid, Platform Platform, bool AllStats)
        {
            return await GetOverallAsync(new[] { Uuid }, Platform, AllStats).ConfigureAwait(false);
        }

        public async Task<Dictionary<string, Statistics>> GetOverallAsync(Guid[] Uuids, Platform Platform, bool AllStats)
        {
            var queries = HttpUtility.ParseQueryString(string.Empty);
            queries.Add("populations", string.Join(',', Uuids));
            queries.Add("statistics", AllStats ? Constants.AllStatisticsVariables : Constants.CoreStatisticsVariables);

            var results = await GetDataAsync<StatisticsFetch>(Endpoints.Overall, Platform, queries.ToString()).ConfigureAwait(false);
            return results.Stats;
        }

        private async Task<T> GetDataAsync<T>(string Url, Platform? Platform, string Queries)
        {
            if (Platform != null)
            {
                Url = string.Format(Url, PlatformToGuid(Platform ?? default));
            }
            if (Queries != null)
            {
                Url = $"{Url}?{Queries.ToString()}";
            }

            var uri = new Uri(Url);
            var response = await GetAuthenticatedRequestAsync(uri).ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(response);
        }

        private static string PlatformToString(Platform Platform)
        {
            return Platform switch
            {
                Platform.PSN => "psn",
                Platform.Uplay => "uplay",
                Platform.XBL => "xbl",
                _ => throw new Exception("Platform does not exist.")
            };
        }

        private static string RegionToString(Region Region)
        {
            return Region switch
            {
                Region.APAC => "apac",
                Region.EMEA => "emea",
                Region.NCSA => "ncsa",
                _ => throw new Exception("Region does not exist."),
            };
        }

        private static Guid PlatformToGuid(Platform Platform)
        {
            return Platform switch
            {
                Platform.Uplay => Constants.Uplay,
                Platform.PSN => Constants.PSN,
                Platform.XBL => Constants.XBL,
                _ => throw new Exception("Platform does not exist."),
            };
        }

        private async Task<string> GetAuthenticatedRequestAsync(Uri uri)
        {
            // Add authorization header with ticket
            var headervaluepairs = new []
            {
                new KeyValuePair<HttpRequestHeader, string>(HttpRequestHeader.Authorization, $"Ubi_v1 t={await GetTicketAsync().ConfigureAwait(false)}")
            };

            // Get result from endpoint
            return await BuildRequestAsync(uri, headervaluepairs, null, true).ConfigureAwait(false);
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
                var response = await BuildRequestAsync(new Uri(Endpoints.Sessions), headervaluepairs, data, false).ConfigureAwait(false);
                CurrentSession = JsonSerializer.Deserialize<Session>(response);
            }

            return CurrentSession.Ticket;
        }

        private static async Task<string> BuildRequestAsync(Uri uri, KeyValuePair<HttpRequestHeader, string>[] AdditionalHeaderValues, byte[] Data, bool Get)
        {
            // Build a web request to endpoint
            var request = WebRequest.CreateHttp(uri);
            // Set request method
            request.Method = Get ? "GET" : "POST";
            // Apply usual request headers that should be in all requests to Ubisoft
            request.Headers.Add(HttpRequestHeader.ContentType, MediaTypeNames.Application.Json);
            request.Headers.Add("Ubi-AppId", Constants.Rainbow6S.ToString());
            // Apply auxiliary headers supplied to method
            foreach (var AdditionalHeaderValue in AdditionalHeaderValues)
            {
                request.Headers.Add(AdditionalHeaderValue.Key, AdditionalHeaderValue.Value);
            }

            // If we have some data to send, write it to stream (make sure it is POST)
            if (Data != null && request.Method.Equals("POST"))
            {
                request.ContentLength = Data.Length;
                using (var stream = request.GetRequestStream())
                {
                    await stream.WriteAsync(Data).ConfigureAwait(false);
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
    }
}
