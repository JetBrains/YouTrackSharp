using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class MultipleProjectStateWrapper: IDataWrapper<ProjectState>
    {
        [JsonName("state")]
        public ProjectState[] Data { get; set; }
        
    }
}