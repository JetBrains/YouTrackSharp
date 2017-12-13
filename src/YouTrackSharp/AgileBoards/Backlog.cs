using Newtonsoft.Json;

namespace YouTrackSharp.AgileBoards
{
    public class Backlog
    {
        [JsonProperty("value")]
        public string Value { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("visibleForGroup")]
        public string VisibleForGroup { get; set; }

        [JsonProperty("updatableByGroup")]
        public object UpdatableByGroup { get; set; }
    }
}