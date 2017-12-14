using Newtonsoft.Json;

namespace YouTrackSharp.AgileBoards
{
    /// <summary>
    /// A class that represents a YouTrack field used in the context of an agile board
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Gets or sets the name of the field
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the localized name of the field
        /// </summary>
        [JsonProperty("localizedName")]
        public string LocalizedName { get; set; }
    }
}