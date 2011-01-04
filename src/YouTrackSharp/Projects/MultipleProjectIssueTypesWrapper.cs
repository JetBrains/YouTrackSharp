using System.Collections.Generic;
using JsonFx.Json;

namespace YouTrackSharp.Projects
{
    public class MultipleProjectIssueTypesWrapper: IDataWrapper<ProjectIssueTypes>
    {
        [JsonName("type")]
        public ProjectIssueTypes[] Data { get; set; }
    }
}