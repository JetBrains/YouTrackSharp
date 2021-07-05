using System;
using Newtonsoft.Json;
using YouTrackSharp.Generated;
using YouTrackSharp.Json;
using YouTrackSharp.Internal;

namespace YouTrackSharp.TimeTracking
{
    /// <summary>
    /// A class that represents YouTrack issue work item information.
    /// </summary>
    public class WorkItem
    {
        /// <summary>
        /// Creates an instance of the <see cref="WorkItem" /> class.
        /// </summary>
        public WorkItem()
        {
        }

        /// <summary>
        /// Creates an instance of the <see cref="WorkItem" /> class.
        /// </summary>
        /// <param name="date"><see cref="T:System.DateTime"/> when the work item is created.</param>
        /// <param name="duration">Duration of the work item.</param>
        /// <param name="description">Description of the work item.</param>
        /// <param name="workType">The <see cref="WorkType"/> for the work item.</param>
        /// <param name="author">Author of the work item. When <value>null</value>, the API caller's identity will be assumed.</param>
        /// <exception cref="ArgumentNullException">When argument is null.</exception>
        public WorkItem(DateTime? date, TimeSpan duration, string description = null, WorkType workType = null, Author author = null)
        {
            Date = date;
            Duration = duration;
            Description = description ?? throw new ArgumentNullException(nameof(description));
            WorkType = workType;
            Author = author;
        }

        /// <summary>
        /// Creates an instance of the <see cref="WorkItem" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="IssueWorkItem"/> to convert from.</param>
        internal static WorkItem FromApiEntity(IssueWorkItem entity)
        {
            return new WorkItem()
            {
                Id = entity.Id,
                Date = entity.Date?.TimestampToDateTime(),
                Duration = entity.Duration.Minutes.MinutesToTimeSpan(),
                Description = entity.Text,
                WorkType = entity.Type == null ? null : WorkType.FromApiEntity(entity.Type),
                Author = entity.Author == null ? null : Author.FromApiEntity(entity.Author)
            };
        }

        /// <summary>
        /// Converts to instance of the <see cref="IssueWorkItem" /> class used in api client.
        /// </summary>
        internal IssueWorkItem ToApiEntity()
        {
            var entity = new IssueWorkItem()
            {
                Date = Date.DateTimeToUnixTimestamp(),
                Duration = new DurationValue() {Minutes = (int)Duration.TotalMinutes},
                Text = Description,
                Type = WorkType.ToApiEntity(),
                Author = Author?.ToApiEntity()
            };

            return entity;
        }

        
        /// <summary>
        /// Id of the work item.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// Represents when the work item was performed.
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        [JsonProperty("date")]
        public DateTime? Date { get; set; }

        /// <summary>
        /// Duration of the work item in minutes.
        /// </summary>
        [JsonConverter(typeof(TimeSpanMinutesConverter))]
        [JsonProperty("duration")]
        public TimeSpan Duration { get; set; }

        /// <summary>
        /// Description of the work item.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Work type of the work item.
        /// </summary>
        [JsonProperty("worktype")]
        public WorkType WorkType { get; set; }

        /// <summary>
        /// Author of the work item.
        /// </summary>
        [JsonProperty("author")]
        public Author Author { get; set; }
        
        /// <summary>
        /// Represents when the work item was created.
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        [JsonProperty("created")]
        public DateTime? Created { get; set; }
        
        /// <summary>
        /// Represents when the work item was updated.
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        [JsonProperty("updated")]
        public DateTime? Updated { get; set; }
    }
}