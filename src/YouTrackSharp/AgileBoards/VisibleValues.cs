using Newtonsoft.Json;

namespace YouTrackSharp.AgileBoards
{
    /// <summary>
    /// A class that represents a visible column in a board
    /// </summary>
    public class VisibleValue
    {
        /// <summary>
        /// Gets or sets the name of the column
        /// </summary>
        [JsonProperty("value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or sets the minimum Work in Progress allowed
        /// </summary>
        [JsonProperty("min")]
        public object Min { get; set; }

        /// <summary>
        /// Gets or sets the maximum Work in Progress allowed
        /// </summary>
        [JsonProperty("max")]
        public object Max { get; set; }
    }
}