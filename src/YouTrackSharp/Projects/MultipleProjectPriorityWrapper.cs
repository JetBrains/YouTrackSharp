using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class MultipleProjectPriorityWrapper : IDataWrapper<ProjectPriority>
    {
        #region IDataWrapper<ProjectPriority> Members

        [JsonName("priority")]
        public ProjectPriority[] Data { get; set; }

        #endregion
    }
}