using Newtonsoft.Json;

namespace YouTrackSharp.AgileBoards
{
    public class VisibleValue
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("min")]
        public object Min { get; set; }

        [JsonProperty("max")]
        public object Max { get; set; }
    }
}