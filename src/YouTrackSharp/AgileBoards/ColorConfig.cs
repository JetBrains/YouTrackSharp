using Newtonsoft.Json;

namespace YouTrackSharp.AgileBoards
{
    public class ColorConfig
    {
        [JsonProperty("field")]
        public Field Field { get; set; }
    }
}