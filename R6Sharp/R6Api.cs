using Newtonsoft.Json;
using R6Sharp.ResponseTypes;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace R6Sharp
{
    public class R6Api
    {
        /// <summary>
        /// Credential in base64 acquired from constructor
        /// </summary>
        private readonly string _credentialsb64;

        // Make this static so it persists in memory over current program instance
        public static Session CurrentSession { get; private set; }

        public R6Api(string email, string password)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentNullException(this.GetType().FullName, "Email address cannot be null or empty.");
            }
            else if (string.IsNullOrWhiteSpace(password))
            {
                throw new ArgumentNullException(this.GetType().FullName, "Password cannot be null or empty.");
            }

            // generate an auth for acquiring a token
            var auth = $"{email}:{password}";
            var bytes = Encoding.UTF8.GetBytes(auth);
            _credentialsb64 = Convert.ToBase64String(bytes);
        }

        public async Task<string> BuildRequestAsync(Uri uri, KeyValuePair<HttpRequestHeader, string>[] AdditionalHeaderValues, byte[] Data, bool Get = true)
        {
            // Build a web request to endpoint
            var request = WebRequest.CreateHttp(uri);
            // Set request method
            request.Method = Get ? "GET" : "POST";
            // Apply usual request headers that should be in all requests to Ubisoft
            request.Headers.Add(HttpRequestHeader.ContentType, MediaTypeNames.Application.Json);
            request.Headers.Add("Ubi-AppId", Constants.RainbowUUID);
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
            {
                using (var stream = response.GetResponseStream())
                {
                    var bytes = new byte[response.ContentLength];
                    stream.Read(bytes);
                    result = Encoding.GetEncoding(response.CharacterSet).GetString(bytes);
                }
            }

            return result;
        }

        public async Task<string> GetTokenAsync(bool rememberMe = true)
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
                byte[] data = Encoding.UTF8.GetBytes($"{{\"rememberMe\": {rememberMe.ToString().ToLower()}}}");
                // Add authorization header
                var headervaluepairs = new KeyValuePair<HttpRequestHeader, string>[]
                {
                    new KeyValuePair<HttpRequestHeader, string>(HttpRequestHeader.Authorization, $"Basic {_credentialsb64}")
                };

                // Get result from endpoint
                var response = await BuildRequestAsync(new Uri(Endpoints.Sessions), headervaluepairs, data, false);
                CurrentSession = JsonConvert.DeserializeObject<Session>(response);
            }

            return rememberMe ? CurrentSession.RememberMeTicket : CurrentSession.Ticket;
        }
    }
}
