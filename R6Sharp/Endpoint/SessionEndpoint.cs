using R6Sharp.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
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
            var sessionFileName = "session.json";
            var requestNewSession = false;
            // If Session exists in memory, check if it has expired
            if (_currentSession != null)
            {
                var now = DateTime.UtcNow;
                if (now >= _currentSession.Expiration)
                {
                    // Session expired, get new session
                    requestNewSession = true;
                }
            }
            else
            {
                // This is the first time starting session, check if session details were saved as JSON
                try
                {
                    var json = File.ReadAllText(sessionFileName);
                    var loadedSession = JsonSerializer.Deserialize<Session>(json);

                    // If Session was readable and there is time before expiration
                    if (loadedSession != null && DateTime.UtcNow < loadedSession.Expiration)
                    {
                        // Use the loaded session
                        _currentSession = loadedSession;
                    }
                    else
                    {
                        // Get new session
                        requestNewSession = true;
                    }
                }
                catch (Exception e)
                {
                    // Session file was not found or it was malformed/invalid
                    if (e is FileNotFoundException || e is JsonException)
                    {
                        requestNewSession = true;
                    }
                    else
                    {
                        // Error is unknown, rethrow it
                        throw e;
                    }
                }
            }

            if (requestNewSession)
            {
                // Refresh current session details (will get new session if expired or non-existent)
                _currentSession = await GetSessionAsync().ConfigureAwait(false);
                // Save new session to file
                var serialisedSession = JsonSerializer.Serialize<Session>(_currentSession);
                File.WriteAllText(sessionFileName, serialisedSession);
            }

            return _currentSession.Ticket;
        }

        private async Task<Session> GetSessionAsync()
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
            return JsonSerializer.Deserialize<Session>(response);
        }
    }
}
