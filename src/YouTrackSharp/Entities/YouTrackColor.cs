using Newtonsoft.Json;
using YouTrackSharp.Generated;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp
{
    /// <summary>
    /// YouTrack color representation that has a foreground and background color in HEX format (e.g. #112233).
    /// </summary>
    public sealed class YouTrackColor
    {
        /// <summary>
        /// Creates an instance of the <see cref="YouTrackColor" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="FieldStyle"/> to convert from.</param>
        internal static YouTrackColor FromApiEntity(FieldStyle entity)
        {
            return entity == null
                ? null
                : new YouTrackColor() {Foreground = entity.Foreground, Background = entity.Background};
        }
        
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