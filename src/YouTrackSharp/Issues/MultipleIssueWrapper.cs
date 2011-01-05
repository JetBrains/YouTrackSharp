using JsonFx.Json;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Issues
{
    public class MultipleIssueWrapper: IDataWrapper<Issue>
    {
        [JsonName("issue")]
        public Issue[] Data { get; set; }

    }
}