using Newtonsoft.Json;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp
{
    /// <summary>
    /// Represents a sub value which contains a <see cref="TSubValueType"/> value.
    /// </summary>
    public struct SubValue<TSubValueType>
    {
        /// <summary>
        /// Value.
        /// </summary>
        [JsonProperty("value")]
        public TSubValueType Value;
    }
}