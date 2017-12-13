using Newtonsoft.Json;
using System.Collections.Generic;

namespace YouTrackSharp.AgileBoards
{
    public class SwimlaneSettings
    {
        [JsonProperty("field")]
        public Field Field { get; set; }

        [JsonProperty("defaultCardType")]
        public string DefaultCardType { get; set; }

        [JsonProperty("values")]
        public ICollection<string> Values { get; set; }
    }
}