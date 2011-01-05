using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class MultipleProjectResolutionTypesWrapper : IDataWrapper<ProjectResolutionTypes>
    {
        #region IDataWrapper<ProjectResolutionTypes> Members

        [JsonName("resolution")]
        public ProjectResolutionTypes[] Data { get; set; }

        #endregion
    }
}