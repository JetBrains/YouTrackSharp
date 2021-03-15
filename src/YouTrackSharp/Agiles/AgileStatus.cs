using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Shows if the board has any configuration problems.
    /// </summary>
    public class AgileStatus
    {
        /// <summary>
        /// Id of the AgileStatus.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// true if the board is in valid state and can be used. Read-only.
        /// </summary>
        [JsonProperty("valid")]
        public bool Valid { get; set; }

        /// <summary>
        /// If `true`, then a background job is currently being executed for the board. In this case, while a background
        /// job is running, the board cannot be updated. Read-only.
        /// </summary>
        [JsonProperty("hasJobs")]
        public bool HasJobs { get; set; }

        /// <summary>
        /// List of configuration errors found for this board. Read-only.
        /// </summary>
        [JsonProperty("errors")]
        public List<string> Errors { get; set; }

        /// <summary>
        /// List of configuration-related warnings found for this board. Read-only.
        /// </summary>
        [JsonProperty("warnings")]
        public List<string> Warnings { get; set; }
    }
}