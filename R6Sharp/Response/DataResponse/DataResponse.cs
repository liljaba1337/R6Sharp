using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using static R6Sharp.Parser;

namespace R6Sharp.Response.DataResponse
{
    public enum DataType { Maps, Operators, Summary, Trend, Weapons };

    public enum RoleType { TeamRoles, TeamRolesWeapons };

    public class DataResponse<T>
    {
        [JsonPropertyName("profileId")]
        public Guid ProfileId { get; set; }

        [JsonPropertyName("region")]
        [JsonConverter(typeof(ParseStringToRegion))]
        public Region Region { get; set; }

        [JsonPropertyName("statType")]
        [JsonConverter(typeof(ParseStringToDataTypeType))]
        public DataType StatType { get; set; }

        [JsonPropertyName("platforms")]
        public Dictionary<string, Platform<T>> Platforms { get; set; }
    }

    public class Platform<T>
    {
        [JsonPropertyName("gameModes")]
        public Dictionary<string, Gamemode<T>> Gamemodes { get; set; }
    }

    public class Gamemode<T>
    {
        [JsonPropertyName("type")]
        [JsonConverter(typeof(ParseStringToRoleType))]
        public RoleType Type { get; set; }

        [JsonPropertyName("teamRoles")]
        public Dictionary<string, T> TeamRoles { get; set; }
    }
}