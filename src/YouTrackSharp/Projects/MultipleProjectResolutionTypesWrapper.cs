using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class MultipleProjectResolutionTypesWrapper: IDataWrapper<ProjectResolutionTypes>
    {
        [JsonName("resolution")]
        public ProjectResolutionTypes[] Data { get; set; }
    }
}