using System;
using Newtonsoft.Json;

namespace YouTrackSharp.TimeTracking
{
    /// <summary>
    /// A class that represents YouTrack issue work type information.
    /// </summary>
    public class WorkType
    {
        /// <summary>
        /// Id of the work type.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// Name of the work type.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Url of the work type.
        /// </summary>
        [JsonProperty("url")]
        public Uri Url { get; set; }
    }
}