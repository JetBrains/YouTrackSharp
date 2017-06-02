using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp
{
    /// <summary>
    /// Represents a sub value which contains a <see cref="T:System.String"/> value.
    /// </summary>
    public struct SubValue
    {
        /// <summary>
        /// Value.
        /// </summary>
        [JsonProperty("value")]
        public string Value;
    }
}