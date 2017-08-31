using Newtonsoft.Json;

namespace YouTrackSharp.Management
{
    /// <summary>
    /// A class that represents YouTrack group information.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Id of the group.
        /// </summary>
        [JsonProperty("entityId")]
        public string Id { get; set; }

        /// <summary>
        /// Name of the group.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// URL of the group.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}