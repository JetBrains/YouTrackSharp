using Newtonsoft.Json;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Represents a custom field of the issue.
    /// </summary>
    public class CustomFilterField : FilterField
    {
        /// <summary>
        /// Reference to settings of the custom field. Read-only.
        /// </summary>
        [JsonProperty("customField")]
        public CustomField CustomField { get; set; }
    }
}