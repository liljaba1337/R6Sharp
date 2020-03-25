using Newtonsoft.Json;
using R6Sharp.ResponseTypes;
using System;
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

        public string Ticket { get; private set; }

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

        public async Task<string> GetTokenAsync(bool rememberMe = true)
        {
            // get token from Ubisoft
            var result = "";

            var request = WebRequest.CreateHttp(new Uri(Endpoints.Sessions));
            request.Method = "POST";
            request.Headers.Add(HttpRequestHeader.ContentType, MediaTypeNames.Application.Json);    // For rememberMe post
            request.Headers.Add(HttpRequestHeader.Authorization, $"Basic {_credentialsb64}");       // Authorization header
            request.Headers.Add("Ubi-AppId", Constants.RainbowUUID);                                // For access

            if (rememberMe)
            {
                var data = Encoding.UTF8.GetBytes("{\"rememberMe\": true}");
                request.ContentLength = data.Length;
                using (var stream = request.GetRequestStream())
                {
                    await stream.WriteAsync(data).ConfigureAwait(false);
                }
            }

            using (var response = (HttpWebResponse)request.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                var bytes = new byte[response.ContentLength];
                stream.Read(bytes);

                var content = Encoding.UTF8.GetString(bytes);
                var json = JsonConvert.DeserializeObject<Session>(content);
                result = rememberMe ? json.RememberMeTicket : json.Ticket;
            }

            Ticket = result;
            return result;
        }
    }
}
