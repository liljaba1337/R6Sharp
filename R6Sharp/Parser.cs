using R6Sharp.Exceptions;
using R6Sharp.Response;
using R6Sharp.Response.Static;
using System;
using System.Buffers;
using System.Buffers.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace R6Sharp
{
    internal class Parser
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

        internal class ParseStringToRegion : JsonConverter<Region>
        {
            public override Region Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var region = reader.GetString();
                if (region.Equals("all"))
                {
                    return Region.All;
                }
                else
                {
                    return Enum.Parse<Region>(region.ToUpper());
                }
            }

            public override void Write(Utf8JsonWriter writer, Region value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        internal class ParseStringToDataResponseType : JsonConverter<DataResponseType>
        {
            public override DataResponseType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var type = reader.GetString();
                return type switch
                {
                    "maps" => DataResponseType.Maps,
                    "operators" => DataResponseType.Operators,
                    "summary" => DataResponseType.Summary,
                    "trend" => DataResponseType.Trend,
                    "weapons" => DataResponseType.Weapons,
                    _ => throw new UnrecognizedDataException($"Could not recognize \"{type}\" as {typeof(DataResponseType).Name}."),
                };
            }

            public override void Write(Utf8JsonWriter writer, DataResponseType value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        internal class ParseStringToDateTime : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var date = DateTime.ParseExact(reader.GetInt32().ToString(), "yyyyMMdd", null);
                return date;
            }

            public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        internal class ParseStringToRoleType : JsonConverter<RoleType>
        {
            public override RoleType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var type = reader.GetString();
                return type switch
                {
                    "Team roles" => RoleType.TeamRoles,
                    "Team roles weapons" => RoleType.TeamRolesWeapons,
                    _ => throw new UnrecognizedDataException($"Could not recognize \"{type}\" as {typeof(RoleType).Name}."),
                };
            }

            public override void Write(Utf8JsonWriter writer, RoleType value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }
    }
}