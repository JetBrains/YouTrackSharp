using System;
using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class ProjectState
    {
        public string Name { get; set; }
        [JsonName("resolved")]
        public Boolean IsResolved { get; set; }
    }
}