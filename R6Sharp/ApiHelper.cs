using R6Sharp.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace R6Sharp
{
    internal static class ApiHelper
    {
        internal static async Task<string> GetDataAsync(string url, Guid player, IEnumerable<KeyValuePair<string, string>> queries, Session session)
        {
            url = string.Format(url, player.ToString());
            return await GetDataAsync(url, queries, session).ConfigureAwait(false);
        }

        internal static async Task<string> GetDataAsync(string url, Platform? platform, IEnumerable<KeyValuePair<string, string>> queries, Session session)
        {
            if (platform != null)
            {
                if (url.Equals(Endpoints.Progressions) || url.Equals(Endpoints.Players) || url.Equals(Endpoints.Statistics))
                {
                    url = string.Format(url, Constant.PlatformToGuid(platform ?? default), Constant.PlatformToSandbox(platform ?? default));
                }
                else
                {
                    url = string.Format(url, Constant.PlatformToGuid(platform ?? default));
                }
            }
            return await GetDataAsync(url, queries, session);
        }

        private static async Task<string> GetDataAsync(string url, IEnumerable<KeyValuePair<string, string>> queries, Session session)
        {
            if (queries != null)
            {
                // TO-DO: find a better, more secure way of doing this
                var completeQueries = new List<string>();
                foreach (var query in queries)
                {
                    completeQueries.Add(string.Join('=', query.Key, query.Value));
                }

                url = string.Join('?', url, string.Join('&', completeQueries));
            }

            var uri = new Uri(url);
            // Add authorization header with ticket (may be null, for requests that are static)
            var headerValuePairs = new KeyValuePair<string, string>[3];
            if (session != null)
            {
                headerValuePairs[0] = new KeyValuePair<string, string>("Authorization", $"Ubi_v1 t={session.Ticket}");
                headerValuePairs[1] = new KeyValuePair<string, string>("Expiration", session.Expiration.ToString("O"));
                headerValuePairs[2] = new KeyValuePair<string, string>("Ubi-SessionID", session.SessionId.ToString());
            }
            return await BuildRequestAsync(uri, headerValuePairs, null, true).ConfigureAwait(false);
        }

        internal static async Task<string> BuildRequestAsync(Uri uri, KeyValuePair<string, string>[] additionalHeaderValues, byte[] data, bool get)
        {
            // Build a web request to endpoint
            var request = WebRequest.CreateHttp(uri);
            // Set request method
            request.Method = get ? WebRequestMethods.Http.Get : WebRequestMethods.Http.Post;
            // Apply usual request headers that should be in all requests to Ubisoft
            if (!uri.AbsoluteUri.Contains("r6s-stats"))
            {
                request.Headers.Add(HttpRequestHeader.ContentType, MediaTypeNames.Application.Json);
            }
            request.Headers.Add("Ubi-AppId", Constant.Rainbow6S.ToString());
            request.Headers.Add(HttpRequestHeader.UserAgent, "R6Sharp/2.0");
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

        internal static string DeriveGamemodeFlags(Gamemode gamemode)
        {
            var gamemodes = new List<string>();
            if (gamemode.HasFlag(Gamemode.Casual)) gamemodes.Add(Constant.GamemodeToString(Gamemode.Casual));
            if (gamemode.HasFlag(Gamemode.Unranked)) gamemodes.Add(Constant.GamemodeToString(Gamemode.Unranked));
            if (gamemode.HasFlag(Gamemode.Ranked)) gamemodes.Add(Constant.GamemodeToString(Gamemode.Ranked));
            if (gamemode.HasFlag(Gamemode.All)) gamemodes.Add(Constant.GamemodeToString(Gamemode.All));
            return string.Join(',', gamemodes);
        }

        internal static string DerivePlatformFlags(Platform platform)
        {
            var platforms = new List<string>();
            if (platform.HasFlag(Platform.PC)) platforms.Add(Constant.PlatformToName(Platform.PC));
            if (platform.HasFlag(Platform.PSN)) platforms.Add(Constant.PlatformToName(Platform.PSN));
            if (platform.HasFlag(Platform.XBL)) platforms.Add(Constant.PlatformToName(Platform.XBL));
            return string.Join(',', platforms);
        }
    }
}
