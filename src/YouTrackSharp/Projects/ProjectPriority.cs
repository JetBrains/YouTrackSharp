using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class ProjectPriority
    {
        public string Name { get; set; }

        [JsonName("priority")]
        public string NumericValue { get; set; }
    }
}