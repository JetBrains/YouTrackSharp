using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YouTrackSharp.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// A class that represents YouTrack issue comment information.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Creates an instance of the <see cref="Comment" /> class.
        /// </summary>
        public Comment()
        {
            Replies = new List<Comment>();
        }

        /// <summary>
        /// Id of the comment.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Author of the comment.
        /// </summary>
        [JsonProperty("author")]
        public string Author { get; set; }

        /// <summary>
        /// Author of the comment (full name).
        /// </summary>
        [JsonProperty("authorFullName")]
        public string AuthorFullName { get; set; }

        /// <summary>
        /// Issue id to which the comment belongs.
        /// </summary>
        [JsonProperty("issueId")]
        public string IssueId { get; set; }

        /// <summary>
        /// Parent comment id.
        /// </summary>
        [JsonProperty("parentId")]
        public string ParentId { get; set; }

        /// <summary>
        /// Is the comment deleted?
        /// </summary>
        [JsonProperty("deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// If comment was imported from JIRA, represents the Id it has in JIRA.
        /// </summary>
        [JsonProperty("jiraId")]
        public string JiraId { get; set; }

        /// <summary>
        /// Text of the comment.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

        /// <summary>
        /// Is the comment shown for the issue author?
        /// </summary>
        [JsonProperty("shownForIssueAuthor")]
        public bool ShownForIssueAuthor { get; set; }

        /// <summary>
        /// Represents when the issue was created.
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        [JsonProperty("created")]
        public DateTime? Created { get; set; }

        /// <summary>
        /// Represents when the issue was updated.
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }

        /// <summary>
        /// Permitted group.
        /// </summary>
        [JsonProperty("permittedGroup")]
        public string PermittedGroup { get; set; }

        /// <summary>
        /// Replies.
        /// </summary>
        [JsonProperty("replies")]
        public ICollection<Comment> Replies { get; set; }
    }
}