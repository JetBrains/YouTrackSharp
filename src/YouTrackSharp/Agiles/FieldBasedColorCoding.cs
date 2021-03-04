using Newtonsoft.Json;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Allows to set card's color based on a value of some custom field.
    /// </summary>
    public class FieldBasedColorCoding : ColorCoding
    {
        /// <summary>
        /// Sets card color based on this custom field. Can be null.
        /// </summary>
        [JsonProperty("prototype")]
        public CustomField Prototype { get; set; }
    }
}