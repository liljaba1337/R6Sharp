using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace R6Sharp.ResponseTypes
{
    public class PlayerInfoSearch
    {
        [JsonPropertyName("profiles")]
        public List<PlayerInfo> Players { get; set; }
    }

    public class PlayerInfo
    {
        [JsonPropertyName("profileId")]
        public Guid ProfileId { get; set; }

        [JsonPropertyName("userId")]
        public Guid UserId { get; set; }

        [JsonPropertyName("platformType")]
        public string PlatformType { get; set; }

        [JsonPropertyName("idOnPlatform")]
        public Guid IdOnPlatform { get; set; }

        [JsonPropertyName("nameOnPlatform")]
        public string NameOnPlatform { get; set; }
    }
}
