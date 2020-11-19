namespace R6Sharp
{
    internal static class UbiconnectEndpoint
    {
        // ?gameMode=all,ranked,unranked,casual&platform=PC&startDate=20200718&endDate=20201115
        internal const string Summary = "https://r6s-stats.ubisoft.com/v1/current/summary/{0}";
        // ?gameMode=all,ranked,unranked,casual&platform=PC&teamRole=all&startDate=20200718&endDate=20201115
        internal const string Weapon = "https://r6s-stats.ubisoft.com/v1/current/summary/{0}";
        // teamRole=all?
        // ?gameMode=all,ranked,unranked,casual&platform=PC&teamRole=attacker,defender&startDate=20200718&endDate=20201115
        internal const string Operator = "https://r6s-stats.ubisoft.com/v1/current/summary/{0}";
        // ?gameMode=all,ranked,unranked,casual&platform=PC&teamRole=all,attacker,defender&startDate=20200718&endDate=20201115
        internal const string Map = "https://r6s-stats.ubisoft.com/v1/current/summary/{0}";
        // ?gameMode=all,ranked,unranked,casual&startDate=20200718&endDate=20201115&teamRole=all,attacker,defender&trendType=weeks
        internal const string Trend = "https://r6s-stats.ubisoft.com/v1/current/summary/{0}";
        // ?gameMode=all,ranked,unranked,casual&platform=PC
        internal const string Seasonal = "https://r6s-stats.ubisoft.com/v1/current/summary/{0}";
        // ?startDate=20200718&endDate=20201115
        internal const string Narrative = "https://r6s-stats.ubisoft.com/v1/current/summary/{0}";
    }

    internal static class Endpoints
    {
        // API access
        internal const string Base = "https://game-rainbow6.ubi.com";
        internal const string Sessions = "https://public-ubiservices.ubi.com/v3/profiles/sessions";
        internal const string Search = "https://public-ubiservices.ubi.com/v2/profiles";

        // These endpoints need to be formatted to correct space uuids (uplay, psn, xbl)
        internal const string Progressions = "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/{1}/r6playerprofile/playerprofile/progressions";
        internal const string Players = "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/{1}/r6karma/players";
        internal const string Statistics = "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/{1}/playerstats2/statistics";

        // These are metadata endpoints, places where static data is stored
        internal const string Avatar = "https://ubisoft-avatars.akamaized.net/{0}/default_146_146.png?appId={1}";
        internal const string Seasons = "https://game-rainbow6.ubi.com/assets/data/seasons.152c15ea.json";
        internal const string Locales = "https://game-rainbow6.ubi.com/assets/locales/locale.{0}.{1}.json";
        internal const string Ranks = "https://game-rainbow6.ubi.com/assets/data/ranks.754ab452.json";
        internal const string Operators = "https://game-rainbow6.ubi.com/assets/data/operators.f660ac39.json";
        internal const string Weapons = "https://game-rainbow6.ubi.com/assets/data/weapons.8a9b3d9e.json";
    }
}
