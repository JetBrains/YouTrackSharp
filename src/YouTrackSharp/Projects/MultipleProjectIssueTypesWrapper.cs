using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class MultipleProjectIssueTypesWrapper : IDataWrapper<ProjectIssueTypes>
    {
        #region IDataWrapper<ProjectIssueTypes> Members

        [JsonName("type")]
        public ProjectIssueTypes[] Data { get; set; }

        #endregion
    }
}