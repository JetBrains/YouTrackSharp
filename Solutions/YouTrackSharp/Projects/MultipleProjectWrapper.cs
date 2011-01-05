using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class MultipleProjectWrapper: IDataWrapper<Project>
    {
        [JsonName("project")]
        public Project[] Data { get; set; }

    }
}