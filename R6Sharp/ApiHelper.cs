using R6Sharp.Exceptions;
using R6Sharp.Response;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;

namespace R6Sharp
{
    internal static class ApiHelper
    {
        internal static async Task<Stream> GetDataAsync(string url, Guid player, IEnumerable<KeyValuePair<string, string>> queries, Session session)
        {
            url = string.Format(url, player.ToString());
            return await GetDataAsync(url, queries, session).ConfigureAwait(false);
        }

        internal static async Task<Stream> GetDataAsync(string url, Platform? platform, IEnumerable<KeyValuePair<string, string>> queries, Session session)
        {
            if (platform != null)
            {
                if (url.Equals(Endpoints.UbiServices.Progressions) ||
                    url.Equals(Endpoints.UbiServices.Players) ||
                    url.Equals(Endpoints.UbiServices.PlayerSkillRecords))
                {
                    url = string.Format(url, Constant.PlatformToGuid(platform ?? default), Constant.PlatformToSandbox(platform ?? default));
                }
                else
                {
                    url = string.Format(url, Constant.PlatformToGuid(platform ?? default));
                }
            }
            return await GetDataAsync(url, queries, session).ConfigureAwait(false);
        }

        private static async Task<Stream> GetDataAsync(string url, IEnumerable<KeyValuePair<string, string>> queries, Session session)
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
            var headerValuePairs = new List<KeyValuePair<string, string>>();
            if (session != null)
            {
                headerValuePairs.Add(new KeyValuePair<string, string>("Authorization", $"Ubi_v1 t={session.Ticket}"));
                headerValuePairs.Add(new KeyValuePair<string, string>("Expiration", session.Expiration.ToString("O")));
                headerValuePairs.Add(new KeyValuePair<string, string>("Ubi-SessionID", session.SessionId.ToString()));
            }

            var result = await BuildRequestAsync(uri, headerValuePairs.ToArray(), null, true).ConfigureAwait(false);
            return EnsureRequestSuccess(result);
        }

        internal static async Task<Tuple<HttpStatusCode, Stream>> BuildRequestAsync(Uri uri, KeyValuePair<string, string>[] additionalHeaderValues, string data, bool get)
        {
            // Build a web request to endpoint
            using var request = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = get ? HttpMethod.Get : HttpMethod.Post,
                Headers =
                {
                    { "Ubi-AppId", Constant.Rainbow6S.ToString() },
                    { "User-Agent", "R6Sharp/2.0" }
                }
            };

            // Apply auxiliary headers supplied to method
            foreach (var additionalHeaderValue in additionalHeaderValues)
            {
                request.Headers.Add(additionalHeaderValue.Key, additionalHeaderValue.Value);
            }

            if (data != null && request.Method == HttpMethod.Post)
            {
                request.Content = new StringContent(data, Encoding.UTF8, MediaTypeNames.Application.Json);
            }

            var client = new HttpClient();
            var response = await client.SendAsync(request).ConfigureAwait(false);
            response = response.EnsureSuccessStatusCode();
            var content = response.Content;
            var stream = await content.ReadAsStreamAsync().ConfigureAwait(false);
            if (response.StatusCode == HttpStatusCode.NoContent || stream == null || (stream != null && stream.Length == 0))
            {
                return new Tuple<HttpStatusCode, Stream>(response.StatusCode, null);
            }
            else
            {
                return new Tuple<HttpStatusCode, Stream>(response.StatusCode, stream);
            }
        }

        private static Stream EnsureRequestSuccess(Tuple<HttpStatusCode, Stream> result)
        {
            // TO-DO: this is potentially ambiguous as only status 200 and 204 is processed and
            // other 200 codes will be processed incorrectly
            if (result.Item1 == HttpStatusCode.NoContent ||
                result.Item1 == HttpStatusCode.OK)
            {
                // BuildRequestAsync already returns null/Stream for NoContent and OK
                // respectively, so return as such.
                return result.Item2;
            }
            else
            {
                throw new ApiBadResponseException($"Bad response from endpoint: status code {result.Item1}.");
            }
        }

        internal static string DeriveGamemodeFlags(Gamemode gamemode)
        {
            var gamemodes = new List<string>();

            if (gamemode.HasFlag(Gamemode.Casual))
            {
                gamemodes.Add(Constant.GamemodeToString(Gamemode.Casual));
            }

            if (gamemode.HasFlag(Gamemode.Unranked))
            {
                gamemodes.Add(Constant.GamemodeToString(Gamemode.Unranked));
            }

            if (gamemode.HasFlag(Gamemode.Ranked))
            {
                gamemodes.Add(Constant.GamemodeToString(Gamemode.Ranked));
            }

            if (gamemode.HasFlag(Gamemode.All))
            {
                gamemodes.Add(Constant.GamemodeToString(Gamemode.All));
            }

            return string.Join(',', gamemodes);
        }

        internal static string DerivePlatformFlags(Platform platform)
        {
            var platforms = new List<string>();

            if (platform.HasFlag(Platform.PC))
            {
                platforms.Add(Constant.PlatformToName(Platform.PC));
            }
            if (platform.HasFlag(Platform.PSN))
            {
                platforms.Add(Constant.PlatformToName(Platform.PSN));
            }
            if (platform.HasFlag(Platform.XBL))
            {
                platforms.Add(Constant.PlatformToName(Platform.XBL));
            }

            return string.Join(',', platforms);
        }

        internal static string DeriveTeamRoleFlags(TeamRole teamrole)
        {
            var teamroles = new List<string>();

            if (teamrole.HasFlag(TeamRole.All))
            {
                teamroles.Add(Constant.TeamRoleToString(TeamRole.All));
            }
            if (teamrole.HasFlag(TeamRole.Attacker))
            {
                teamroles.Add(Constant.TeamRoleToString(TeamRole.Attacker));
            }
            if (teamrole.HasFlag(TeamRole.Defender))
            {
                teamroles.Add(Constant.TeamRoleToString(TeamRole.Defender));
            }

            return string.Join(',', teamroles);
        }
    }
}
