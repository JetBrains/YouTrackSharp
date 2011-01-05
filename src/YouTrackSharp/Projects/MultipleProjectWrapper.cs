using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class MultipleProjectWrapper : IDataWrapper<Project>
    {
        #region IDataWrapper<Project> Members

        [JsonName("project")]
        public Project[] Data { get; set; }

        #endregion
    }
}