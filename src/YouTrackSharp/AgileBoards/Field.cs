using Newtonsoft.Json;

namespace YouTrackSharp.AgileBoards
{
    public class Field
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("localizedName")]
        public string LocalizedName { get; set; }
    }
}