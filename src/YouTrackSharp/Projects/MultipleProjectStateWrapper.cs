using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class MultipleProjectStateWrapper : IDataWrapper<ProjectState>
    {
        #region IDataWrapper<ProjectState> Members

        [JsonName("state")]
        public ProjectState[] Data { get; set; }

        #endregion
    }
}