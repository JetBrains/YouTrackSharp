using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class MultipleProjectPriorityWrapper: IDataWrapper<ProjectPriority>
    {
        [JsonName("priority")]
        public ProjectPriority[] Data { get; set; }
    }
}