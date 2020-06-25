using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Threading.Tasks;

namespace R6Sharp
{
    internal static class ApiHelper
    {

        internal static async Task<T> GetDataAsync<T>(string url, Platform? platform, NameValueCollection queries, string ticket)
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
                new KeyValuePair<HttpRequestHeader, string>(HttpRequestHeader.Authorization, $"Ubi_v1 t={ticket}")
            };
            var response = await BuildRequestAsync(uri, headerValuePairs, null, true).ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(response);
        }

        internal static async Task<string> BuildRequestAsync(Uri uri, KeyValuePair<HttpRequestHeader, string>[] additionalHeaderValues, byte[] data, bool get)
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
    }
}
