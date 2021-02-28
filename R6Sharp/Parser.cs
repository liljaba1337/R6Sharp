using R6Sharp.Exceptions;
using R6Sharp.Response.DataResponse;
using R6Sharp.Response.Statistic;
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

        internal class ParseStringToDataTypeType : JsonConverter<DataType>
        {
            public override DataType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var type = reader.GetString();
                return type switch
                {
                    "maps" => DataType.Maps,
                    "operators" => DataType.Operators,
                    "summary" => DataType.Summary,
                    "trend" => DataType.Trend,
                    "weapons" => DataType.Weapons,
                    _ => throw new UnrecognizedDataException($"Could not recognize \"{type}\" as {typeof(DataType).Name}."),
                };
            }

            public override void Write(Utf8JsonWriter writer, DataType value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        internal class ParseStringToDateTime : JsonConverter<DateTime>
        {
            public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                string value;
                if (reader.TokenType == JsonTokenType.String)
                {
                    value = reader.GetString();
                }
                else
                {
                    value = reader.GetInt32().ToString();
                }

                try
                {
                    var date = DateTime.ParseExact(value, "yyyyMMdd", null);
                    return date;
                }
                catch (FormatException)
                {
                    var date = DateTime.ParseExact(value, "yyyy-MM-dd HH:mm:ss.FFF", null);
                    return date;
                }
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

        internal class ParseStringToSpecificDataTypeType : JsonConverter<SpecificDataType>
        {
            public override SpecificDataType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var type = reader.GetString();
                return type switch
                {
                    "Generalized" => SpecificDataType.Generalized,
                    "Seasonal" => SpecificDataType.Seasonal,
                    _ => throw new UnrecognizedDataException($"Could not recognize \"{type}\" as {typeof(RoleType).Name}."),
                };
            }

            public override void Write(Utf8JsonWriter writer, SpecificDataType value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        internal class ParseStringToTrendDataTypeType : JsonConverter<TrendDataType>
        {
            public override TrendDataType Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var type = reader.GetString();
                if (type == "Trend Blocks")
                {
                    return TrendDataType.TrendBlocks;
                }
                else
                {
                    throw new UnrecognizedDataException($"Could not recognize \"{type}\" as {typeof(TrendDataType).Name}.");
                }
            }

            public override void Write(Utf8JsonWriter writer, TrendDataType value, JsonSerializerOptions options)
            {
                writer.WriteStringValue(value.ToString());
            }
        }

        internal class ParseStringToPlatform : JsonConverter<Platform>
        {
            public override Platform Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
            {
                var platform = reader.GetString();
                return platform switch
                {
                    "uplay" => Platform.PC,
                    "psn" => Platform.PSN,
                    "xbl" => Platform.XBL,
                    _ => throw new UnrecognizedDataException($"Could not recognize \"{platform}\" as {typeof(Platform).Name}.")
                };
            }

            public override void Write(Utf8JsonWriter writer, Platform value, JsonSerializerOptions options)
            {
                throw new NotImplementedException();
            }
        }
    }
}