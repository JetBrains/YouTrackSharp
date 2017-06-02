using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Represents a YouTrack color.
    /// </summary>
    public class FieldColor
    {
        /// <summary>
        /// Background.
        /// </summary>
        [JsonProperty("bg")]
        public string BackgroundHex;

        /// <summary>
        /// Foreground.
        /// </summary>
        [JsonProperty("fg")]
        public string ForegroundHex;
    }
}