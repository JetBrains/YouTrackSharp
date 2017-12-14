using Newtonsoft.Json;

namespace YouTrackSharp.AgileBoards
{
    /// <summary>
    /// A class that represents the color configuration for an agile board
    /// </summary>
    public class ColorConfig
    {
        /// <summary>
        /// Gets or sets the field used to determine how to color code the cards on the board
        /// </summary>
        [JsonProperty("field")]
        public Field Field { get; set; }
    }
}