using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace R6Sharp.ResponseTypes
{
    public class Session
    {
        [JsonProperty("platformType")]
        public string PlatformType { get; private set; }

        [JsonProperty("ticket")]
        public string Ticket { get; private set; }

        [JsonProperty("twoFactorAuthenticationTicket")]
        public string? TwoFactorAuthenticationTicket { get; private set; }

        [JsonProperty("profileId")]
        public Guid ProfileId { get; private set; }

        [JsonProperty("userId")]
        public Guid UserId { get; private set; }

        [JsonProperty("nameOnPlatform")]
        public string NameOnPlatform { get; private set; }

        [JsonProperty("environment")]
        public string Environment { get; private set; }

        [JsonProperty("expiration")]
        public DateTime Expiration { get; private set; }

        [JsonProperty("clientIp")]
        public string ClientIp { get; private set; }

        [JsonProperty("clientIpCountry")]
        public string ClientIpCountry { get; private set; }

        [JsonProperty("serverTime")]
        public DateTime ServerTime { get; private set; }

        [JsonProperty("sessionId")]
        public Guid SessionId { get; private set; }

        [JsonProperty("sessionKey")]
        public string SessionKey { get; private set; }

        [JsonProperty("rememberMeTicket")]
        public string? RememberMeTicket { get; private set; }
    }
}
