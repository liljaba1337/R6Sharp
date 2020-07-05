using R6Sharp.Response.Static;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace R6Sharp
{
    internal static class ApiHelper
    {
        /// <summary>
        /// Parses string to int from JSON strings.
        /// </summary>
        /// Solution by VahidN/Stack Overflow: https://stackoverflow.com/a/59322077/4339019
        internal class ParseStringToInt : JsonConverter<int>
        {
            public override int Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    ReadOnlySpan<byte> span = reader.HasValueSequence ? reader.ValueSequence.ToArray() : reader.ValueSpan;
                    if (Utf8Parser.TryParse(span, out int number, out int bytesConsumed) && span.Length == bytesConsumed)
                    {
                        return number;
                    }

                    if (int.TryParse(reader.GetString(), out number))
                    {
                        return number;
                    }
                }

                return reader.GetInt32();
            }

            public override void Write(Utf8JsonWriter writer, int value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        internal class ParseStringToUri : JsonConverter<Uri>
        {
            public override Uri Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    if (Uri.TryCreate(reader.GetString(), UriKind.RelativeOrAbsolute, out Uri uri))
                    {
                        return uri;
                    }
                }

                return new Uri(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, Uri value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        internal class ParseStringToId : JsonConverter<RankId>
        {
            public override RankId Read(ref Utf8JsonReader reader, Type type, JsonSerializerOptions options)
            {
                if (reader.TokenType == JsonTokenType.String)
                {
                    if (Enum.TryParse(reader.GetString(), true, out RankId id))
                    {
                        return id;
                    }
                }

                return Enum.Parse<RankId>(reader.GetString());
            }

            public override void Write(Utf8JsonWriter writer, RankId value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

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
            // Add authorization header with ticket (may be null, for requests that are static)
            var headerValuePairs = new KeyValuePair<HttpRequestHeader, string>[1];
            if (ticket != null)
            {
                headerValuePairs[0] = new KeyValuePair<HttpRequestHeader, string>(HttpRequestHeader.Authorization, $"Ubi_v1 t={ticket}");
            }
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
