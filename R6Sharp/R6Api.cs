using R6Sharp.Endpoint;
using System;
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
        public readonly ProfileEndpoint Profile;
        public readonly PlayerProgressionEndpoint PlayerProgression;
        public readonly PlayerEndpoint Player;
        public readonly StatisticEndpoint Statistic;
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

            var _session = new SessionEndpoint(email, password, rememberMe);
            Profile = new ProfileEndpoint(_session);
            PlayerProgression = new PlayerProgressionEndpoint(_session);
            Player = new PlayerEndpoint(_session);
            Statistic = new StatisticEndpoint(_session);
            Static = new StaticEndpoint();
        }
        #endregion
    }
}
