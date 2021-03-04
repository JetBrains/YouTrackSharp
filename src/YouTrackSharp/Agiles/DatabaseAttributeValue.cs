using Newtonsoft.Json;
using YouTrackSharp.SerializationAttributes;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Represents string reference to the value.
    /// </summary>
    [KnownType(typeof(SwimlaneEntityAttributeValue))]
    [KnownType(typeof(AgileColumnFieldValue))]
    public class DatabaseAttributeValue
    {
        /// <summary>
        /// Id of the DatabaseAttributeValue.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}