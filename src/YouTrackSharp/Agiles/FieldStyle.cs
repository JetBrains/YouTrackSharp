using Newtonsoft.Json;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Represents the style settings of the field in YouTrack.
    /// </summary>
    public class FieldStyle
    {
        /// <summary>
        /// Id of the FieldStyle.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Background color. Read-only. Can be null.
        /// </summary>
        [JsonProperty("background")]
        public string Background { get; set; }

        /// <summary>
        /// Foreground color. Read-only. Can be null.
        /// </summary>
        [JsonProperty("foreground")]
        public string Foreground { get; set; }
    }
}