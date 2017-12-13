using Newtonsoft.Json;
using System;
using YouTrackSharp.Json;

namespace YouTrackSharp.AgileBoards
{
    public class Sprint
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("version")]
        public string Version { get; set; }

        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        [JsonProperty("start")]
        public DateTime? Start { get; set; }

        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        [JsonProperty("finish")]
        public DateTime? Finish { get; set; }
    }
}