using JsonFx.Json;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Issues
{
    public class MultipleIssueWrapper : IDataWrapper<Issue>
    {
        #region IDataWrapper<Issue> Members

        [JsonName("issue")]
        public Issue[] Data { get; set; }

        #endregion
    }
}