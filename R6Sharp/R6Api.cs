using R6Sharp.Endpoint;
using R6Sharp.Response;
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Threading.Tasks;

namespace R6Sharp
{
    /// <summary>
    /// Region the player is based in.
    /// </summary>
    public enum Region
    {
        All,    // Unified regions
        APAC,   // Asia Pacific :(
        EMEA,   // Europe, Middle East and Africa
        NCSA    // North, Central and South America
    }

    /// <summary>
    /// Platform the player is based on.
    /// </summary>
    [Flags]
    public enum Platform
    {
        PC = 1,     // PC/Uplay
        PSN = 2,    // Playstation Network
        XBL = 4     // Xbox Live
    }

    /// <summary>
    /// Gamemode data to retrieve for player.
    /// </summary>
    [Flags]
    public enum Gamemode
    {
        Casual = 1,
        Unranked = 2,
        Ranked = 4,
        All = 8
    }

    [Flags]
    public enum TeamRole
    {
        All = 1,
        Attacker = 2,
        Defender = 4
    }

    public enum TrendType
    {
        Weeks
    }

    public class R6Api
    {
        private readonly SessionEndpoint _session;

        public readonly ProfileEndpoint Profile;
        public readonly PlayerProgressionEndpoint PlayerProgression;
        public readonly PlayerEndpoint Player;
        public readonly StaticEndpoint Static;

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

            _session = new SessionEndpoint(email, password, rememberMe);
            Profile = new ProfileEndpoint(_session);
            PlayerProgression = new PlayerProgressionEndpoint(_session);
            Player = new PlayerEndpoint(_session);
            Static = new StaticEndpoint();
        }
        #endregion

        public async Task<DataResponse> GetSummaryAsync(Guid uuid, Gamemode gamemodes, Platform platforms, DateTime start, DateTime end)
        {
            var queries = BuildQuery(gamemodes, platforms, start, end, null, null);
            return await GetData<DataResponse>(UbiconnectEndpoint.Summary, uuid, queries);
        }

        public async Task<DataResponse> GetOperatorAsync(Guid uuid, Gamemode gamemodes, Platform platforms, TeamRole teamroles, DateTime start, DateTime end)
        {
            var queries = BuildQuery(gamemodes, platforms, start, end, teamroles, null);
            return await GetData<DataResponse>(UbiconnectEndpoint.Operator, uuid, queries);
        }

        public async Task<DataResponse> GetMapAsync(Guid uuid, Gamemode gamemodes, Platform platforms, TeamRole teamroles, DateTime start, DateTime end)
        {
            var queries = BuildQuery(gamemodes, platforms, start, end, teamroles, null);
            return await GetData<DataResponse>(UbiconnectEndpoint.Map, uuid, queries);
        }

        public async Task<DataResponse> GetWeaponAsync(Guid uuid, Gamemode gamemodes, Platform platforms, TeamRole teamroles, DateTime start, DateTime end)
        {
            throw new NotImplementedException();
        }

        public async Task<DataResponse> GetTrendAsync(Guid uuid, Gamemode gamemodes, DateTime start, DateTime end, TeamRole teamroles, TrendType trendType)
        {
            throw new NotImplementedException();
        }

        private List<KeyValuePair<string, string>> BuildQuery(Gamemode gamemodes,
                                                              Platform platforms,
                                                              DateTime start,
                                                              DateTime end,
                                                              TeamRole? teamroles,
                                                              TrendType? trend)
        {
            var queries = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("gameMode", ApiHelper.DeriveGamemodeFlags(gamemodes)),
                new KeyValuePair<string, string>("platform", ApiHelper.DerivePlatformFlags(platforms)),
                new KeyValuePair<string, string>("startDate", start.ToString("yyyyMMdd")),
                new KeyValuePair<string, string>("endDate", end.ToString("yyyyMMdd"))
            };

            if (teamroles.HasValue)
            {
                var flags = ApiHelper.DeriveTeamRoleFlags(teamroles.Value);
                var query = new KeyValuePair<string, string>("teamRole", flags);
                queries.Add(query);
            }

            if (trend.HasValue)
            {
                var query = new KeyValuePair<string, string>("trendType", trend.Value.ToString().ToLower());
                queries.Add(query);
            }

            return queries;
        }

        private async Task<T> GetData<T>(string endpoint, Guid uuid, List<KeyValuePair<string, string>> queries)
        {
            var session = await _session.GetCurrentSessionAsync().ConfigureAwait(false);
            var results = await ApiHelper.GetDataAsync(endpoint, uuid, queries, session).ConfigureAwait(false);
            var deserialised = JsonSerializer.Deserialize<T>(results);
            return deserialised;
        }
    }
}