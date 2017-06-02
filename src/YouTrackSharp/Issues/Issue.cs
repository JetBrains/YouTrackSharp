using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    // TODO dynamic shizzle - https://github.com/JetBrains/YouTrackSharp/blob/master/src/YouTrackSharp/Issues/Issue.cs
    // TODO strong type EVERYTHING

    /// <summary>
    /// A class that represents YouTrack issue information.
    /// </summary>
    public class Issue
    {
        /// <summary>
        /// Creates an instance of the <see cref="Issue" /> class.
        /// </summary>
        public Issue()
        {
            Fields = new List<Field>();
            Comments = new List<Comment>();
            Tags = new List<string>();
        }

        /// <summary>
        /// Id of the issue.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Short name of the issue's project.
        /// </summary>
        [JsonProperty("projectShortName")]
        public string ProjectShortName { get; set; }

        /// <summary>
        /// Entity Id internal to YouTrack.
        /// </summary>
        [JsonProperty("entityId")]
        public string EntityId { get; set; }

        /// <summary>
        /// If issue was imported from JIRA, represents the Id it has in JIRA.
        /// </summary>
        [JsonProperty("jiraId")]
        public string JiraId { get; set; }

        /// <summary>
        /// Issue fields.
        /// </summary>
        [JsonProperty("field")]
        public ICollection<Field> Fields { get; set; }

        /// <summary>
        /// Issue comments.
        /// </summary>
        [JsonProperty("comment")]
        public ICollection<Comment> Comments { get; set; }

        /// <summary>
        /// Issue tags.
        /// </summary>
        [JsonProperty("tag")]
        public ICollection<string> Tags { get; set; }
    }
}