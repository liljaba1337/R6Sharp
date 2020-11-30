using System.Text.Json.Serialization;

namespace R6Sharp.Response.Statistic
{
    public class WeaponStatistics
    {
        [JsonPropertyName("weaponSlots")]
        public WeaponSlots WeaponSlots { get; set; }
    }

    public class WeaponSlots
    {
        [JsonPropertyName("secondaryWeapons")]
        public AryWeapons SecondaryWeapons { get; set; }

        [JsonPropertyName("primaryWeapons")]
        public AryWeapons PrimaryWeapons { get; set; }
    }

    public class AryWeapons
    {
        [JsonPropertyName("weaponTypes")]
        public WeaponType[] WeaponTypes { get; set; }
    }

    public class WeaponType
    {
        [JsonPropertyName("weaponType")]
        public string WeaponTypeType { get; set; }

        [JsonPropertyName("weapons")]
        public Weapon[] Weapons { get; set; }
    }

    public class Weapon
    {
        [JsonPropertyName("weaponName")]
        public string WeaponName { get; set; }

        [JsonPropertyName("roundsPlayed")]
        public int RoundsPlayed { get; set; }

        [JsonPropertyName("roundsWon")]
        public int RoundsWon { get; set; }

        [JsonPropertyName("roundsLost")]
        public int RoundsLost { get; set; }

        [JsonPropertyName("kills")]
        public int Kills { get; set; }

        [JsonPropertyName("headshots")]
        public int Headshots { get; set; }

        [JsonPropertyName("headshotAccuracy")]
        public double HeadshotAccuracy { get; set; }

        [JsonPropertyName("roundsWithAKill")]
        public double RoundsWithAKill { get; set; }

        [JsonPropertyName("roundsWithMultiKill")]
        public double RoundsWithMultiKill { get; set; }
    }
}
