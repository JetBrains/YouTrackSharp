using Newtonsoft.Json;

namespace YouTrackSharp.AgileBoards
{
    /// <summary>
    /// A class that represents the saved search used to get the backlog for the board
    /// </summary>
    public class Backlog
    {
        /// <summary>
        /// Gets or sets the query used in the search
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the name of hte saved search
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the name of the group that can use the saved search
        /// </summary>
        [JsonProperty("visibleForGroup")]
        public string VisibleForGroup { get; set; }

        /// <summary>
        /// Gets or sets the name of the group that can update the saved search
        /// </summary>
        [JsonProperty("updatableByGroup")]
        public string UpdatableByGroup { get; set; }
    }
}