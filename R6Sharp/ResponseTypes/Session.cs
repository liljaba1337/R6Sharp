using System;
using System.Text.Json.Serialization;

namespace R6Sharp.ResponseTypes
{
    internal class Session
    {
        [JsonPropertyName("platformType")]
        internal string PlatformType { get; set; }

        [JsonPropertyName("ticket")]
        internal string Ticket { get; set; }

        [JsonPropertyName("profileId")]
        internal Guid ProfileId { get; set; }

        [JsonPropertyName("userId")]
        internal Guid UserId { get; set; }

        [JsonPropertyName("nameOnPlatform")]
        internal string NameOnPlatform { get; set; }

        [JsonPropertyName("environment")]
        internal string Environment { get; set; }

        [JsonPropertyName("expiration")]
        internal DateTime Expiration { get; set; }

        [JsonPropertyName("clientIp")]
        internal string ClientIp { get; set; }

        [JsonPropertyName("clientIpCountry")]
        internal string ClientIpCountry { get; set; }

        [JsonPropertyName("serverTime")]
        internal DateTime ServerTime { get; set; }

        [JsonPropertyName("sessionId")]
        internal Guid SessionId { get; set; }

        [JsonPropertyName("sessionKey")]
        internal string SessionKey { get; set; }

#nullable enable
        [JsonPropertyName("twoFactorAuthenticationTicket")]
        internal string? TwoFactorAuthenticationTicket { get; set; }

        [JsonPropertyName("rememberMeTicket")]
        internal string? RememberMeTicket { get; set; }
#nullable disable
    }
}
