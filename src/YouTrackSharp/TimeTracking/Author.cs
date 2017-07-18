using System;
using Newtonsoft.Json;

namespace YouTrackSharp.TimeTracking
{
    /// <summary>
    /// A class that represents YouTrack issue work item author information.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Login.
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; }

        /// <summary>
        /// Uri.
        /// </summary>
        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}