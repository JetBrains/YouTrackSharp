using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Represents a YouTrack issue field.
    /// </summary>
    public class Field
    {
        /// <summary>
        /// Name.
        /// </summary>
        [JsonProperty("name")]
        public string Name;

        /// <summary>
        /// Value.
        /// </summary>
        [JsonProperty("value")]
        public object Value;

        /// <summary>
        /// Value Id.
        /// </summary>
        [JsonProperty("valueId")]
        public object ValueId;

        /// <summary>
        /// Field color.
        /// </summary>
        [JsonProperty("color")]
        public FieldColor Color;
    }
}