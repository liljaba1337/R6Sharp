namespace R6Sharp
{
    internal static class Endpoints
    {
        // API access
        internal const string Base =
            "https://game-rainbow6.ubi.com";

        internal static class UbiServices
        {
            internal const string Sessions =
                "https://public-ubiservices.ubi.com/v3/profiles/sessions";
            internal const string Search =
                "https://public-ubiservices.ubi.com/v2/profiles";
            // These endpoints need to be formatted to correct space uuids (uplay, psn, xbl)
            internal const string Progressions =
                "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/{1}/r6playerprofile/playerprofile/progressions";
            internal const string Players =
                "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/{1}/r6karma/players";
            internal const string PlayerSkillRecords =
                "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/{1}/r6karma/player_skill_records";
            internal const string Statistics =
                "https://public-ubiservices.ubi.com/v1/spaces/{0}/sandboxes/{1}/playerstats2/statistics";
        }

        internal static class R6SStats
        {
            // These endpoints appear to have started tracking approximately from 16/06/2020 and onwards.
            // This was manually verified by manually decrementing the date until no changes were detected in data.
            internal const string Summary =
                "https://r6s-stats.ubisoft.com/v1/current/summary/{0}";
            internal const string Weapon =
                "https://r6s-stats.ubisoft.com/v1/current/weapons/{0}";
            internal const string Operator =
                "https://r6s-stats.ubisoft.com/v1/current/operators/{0}";
            internal const string Map =
                "https://r6s-stats.ubisoft.com/v1/current/maps/{0}";
            internal const string Trend =
                "https://r6s-stats.ubisoft.com/v1/current/trend/{0}";
            internal const string Seasonal =
                "https://r6s-stats.ubisoft.com/v1/seasonal/summary/{0}";
            internal const string Narrative =
                "https://r6s-stats.ubisoft.com/v1/narrative/bestmatchweekly/{0}";
        }

        // These are metadata endpoints, places where static data is stored
        internal static class Static
        {
            internal const string Avatar =
                "https://ubisoft-avatars.akamaized.net/{0}/default_146_146.png?appId={1}";
        }
    }
}