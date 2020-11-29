using System.Text.Json.Serialization;
using static R6Sharp.Parser;

namespace R6Sharp.Response
{
    public class PlayersSkillRecords
    {
        [JsonPropertyName("seasons_player_skill_records")]
        public SeasonsPlayerSkillRecord[] SeasonsPlayerSkillRecords { get; set; }
    }

    public class SeasonsPlayerSkillRecord
    {
        [JsonPropertyName("season_id")]
        public int SeasonId { get; set; }

        [JsonPropertyName("regions_player_skill_records")]
        public RegionsPlayerSkillRecord[] RegionsPlayerSkillRecords { get; set; }
    }

    public class RegionsPlayerSkillRecord
    {
        [JsonPropertyName("region_id")]
        [JsonConverter(typeof(ParseStringToRegion))]
        public Region RegionId { get; set; }

        [JsonPropertyName("boards_player_skill_records")]
        public BoardsPlayerSkillRecord[] BoardsPlayerSkillRecords { get; set; }
    }

    public class BoardsPlayerSkillRecord
    {
        [JsonPropertyName("board_id")]
        public string BoardId { get; set; }

        [JsonPropertyName("players_skill_records")]
        public BoardInfo[] PlayerSkillRecords { get; set; }
    }
}