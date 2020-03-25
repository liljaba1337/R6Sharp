using System;

namespace R6Sharp
{
    internal static class Endpoints
    {
        internal const string Sessions = "https://public-ubiservices.ubi.com/v3/profiles/sessions";
        internal const string Search = "https://public-ubiservices.ubi.com/v2/profiles";

        // These endpoints need to be formatted to correct space uuids (uplay, psn, xbl)
        internal const string Profile = "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/OSBOR_PC_LNCH_A/r6playerprofile/playerprofile/progressions";
        internal const string Ranked = "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/OSBOR_PC_LNCH_A/r6karma/players";
    }
}
