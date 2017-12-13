using Newtonsoft.Json;

namespace YouTrackSharp.AgileBoards
{
    public class Project
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}