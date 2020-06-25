using System;

namespace R6Sharp
{
    internal static class Endpoint
    {
        // API access
        internal const string Sessions = "https://public-ubiservices.ubi.com/v3/profiles/sessions";
        internal const string Search = "https://public-ubiservices.ubi.com/v2/profiles";

        // These endpoints need to be formatted to correct space uuids (uplay, psn, xbl)
        internal const string Progressions = "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/OSBOR_PC_LNCH_A/r6playerprofile/playerprofile/progressions";
        internal const string Ranked = "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/OSBOR_PC_LNCH_A/r6karma/players";
        internal const string Statistics = "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/OSBOR_PC_LNCH_A/playerstats2/statistics";

        // These are metadata endpoints, places where static data is stored
        internal const string Avatar = "https://ubisoft-avatars.akamaized.net/{0}/default_146_146.png?appId={1}";
        internal const string Seasons = "https://game-rainbow6.ubi.com/assets/data/seasons.152c15ea.json";
        internal const string Locales = "https://game-rainbow6.ubi.com/assets/locales/locale.en-us.6eb4f5cd.json";
        internal const string Ranks = "https://game-rainbow6.ubi.com/assets/data/ranks.754ab452.json";
        internal const string Operators = "https://game-rainbow6.ubi.com/assets/data/operators.f660ac39.json";
        internal const string Weapons = "https://game-rainbow6.ubi.com/assets/data/weapons.8a9b3d9e.json";
    }
}
