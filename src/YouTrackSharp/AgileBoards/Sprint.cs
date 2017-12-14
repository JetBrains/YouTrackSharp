using Newtonsoft.Json;
using System;
using YouTrackSharp.Json;

namespace YouTrackSharp.AgileBoards
{
    /// <summary>
    /// A class that represents a sprint
    /// </summary>
    public class Sprint
    {
        /// <summary>
        /// Gets or sets the id used by YouTrack to identify a sprint
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the name of the sprint.
        /// </summary>
        [JsonProperty("version")]
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets the start date of the sprint
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        [JsonProperty("start")]
        public DateTimeOffset? Start { get; set; }

        /// <summary>
        /// Gets or sets the last day of the sprint
        /// </summary>
        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        [JsonProperty("finish")]
        public DateTimeOffset? Finish { get; set; }
    }
}