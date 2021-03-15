using Newtonsoft.Json;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Represents a field value or values, parameterizing agile column.
    /// </summary>
    public class AgileColumnFieldValue : DatabaseAttributeValue
    {
        /// <summary>
        /// Presentation of a field value or values. Can be null.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// True, if field has type State and teh value is resolved or all values are resolved. Read-only.
        /// </summary>
        [JsonProperty("isResolved")]
        public bool IsResolved { get; set; }
    }
}