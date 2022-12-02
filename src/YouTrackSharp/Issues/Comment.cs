using System;
using Newtonsoft.Json;
using YouTrackSharp.Generated;
using YouTrackSharp.Internal;
using YouTrackSharp.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// A class that represents YouTrack issue comment information.
    /// </summary>
    public class Comment
    {
        /// <summary>
        /// Creates an instance of the <see cref="Comment" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="IssueComment"/> to convert from.</param>
        /// <param name="wikify">If set to <value>true</value>, then comment text will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        internal static Comment FromApiEntity(IssueComment entity, bool wikify = false)
        {
            return new Comment()
            {
                Id = entity.Id,
                Author = entity.Author?.Login,
                AuthorFullName = entity.Author?.FullName,
                IssueId = entity.Issue?.IdReadable,
                IsDeleted = entity.Deleted ?? false,
                IsMarkdown = true,
                Text = wikify ? entity.TextPreview : entity.Text,
                Created = (entity.Created ?? 0).TimestampToDateTime(),
                Updated = (entity.Updated ?? 0).TimestampToDateTime(),
                PermittedGroup = entity.Visibility?.ToSinglePermittedGroup()
            };
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
        /// Is the comment deleted?
        /// </summary>
        [JsonProperty("deleted")]
        public bool IsDeleted { get; set; }

        /// <summary>
        /// Is the comment in Markdown format?
        /// </summary>
        [JsonProperty("markdown")]
        public bool IsMarkdown { get; set; }

        /// <summary>
        /// Text of the comment.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }

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
    }
}