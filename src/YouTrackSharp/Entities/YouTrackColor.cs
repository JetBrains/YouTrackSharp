using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp
{
    /// <summary>
    /// YouTrack color representation that has a foreground and background color in HEX format (e.g. #112233).
    /// </summary>
    public sealed class YouTrackColor
    {
        /// <summary>
        /// Foreground color in HEX format (e.g. #112233).
        /// </summary>
        [JsonProperty("fg")]
        public string Foreground { get; set; }

        /// <summary>
        /// Background color in HEX format (e.g. #112233).
        /// </summary>
        [JsonProperty("bg")]
        public string Background { get; set; }
    }
}