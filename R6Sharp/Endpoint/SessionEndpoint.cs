using R6Sharp.Response;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace R6Sharp.Endpoint
{
    public class SessionEndpoint
    {
        /// <summary>
        /// Credential in base64 acquired from constructor.
        /// </summary>
        private readonly string _credentialsb64;

        /// <summary>
        /// Current session details with Ubisoft.
        /// </summary>
        private Session _currentSession;

        /// <summary>
        /// Control if Ubisoft's session handler should remember current session.
        /// </summary>
        public bool RememberMe { get; set; }

        public SessionEndpoint(string email, string password, bool rememberMe)
        {
            RememberMe = rememberMe;
            // Generate an auth for acquiring a token
            var auth = $"{email}:{password}";
            var bytes = Encoding.UTF8.GetBytes(auth);
            _credentialsb64 = Convert.ToBase64String(bytes);
        }

        public async Task<string> GetTicketAsync()
        {
            // Refresh current session details (will get new session if expired or non-existent)
            _currentSession = await GetSessionAsync().ConfigureAwait(false);
            return _currentSession.Ticket;
        }

        private async Task<Session> GetSessionAsync()
        {
            bool expired = true;
            if (_currentSession != null)
            {
                var now = DateTime.UtcNow;
                if (now < _currentSession.Expiration)
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
                var headervaluepairs = new[]
                {
                    new KeyValuePair<HttpRequestHeader, string>(HttpRequestHeader.Authorization, $"Basic {_credentialsb64}")
                };

                // Get result from endpoint
                var response = await ApiHelper.BuildRequestAsync(new Uri(Endpoints.Sessions), headervaluepairs, data, false).ConfigureAwait(false);
                _currentSession = JsonSerializer.Deserialize<Session>(response);
            }

            return _currentSession;
        }
    }
}
