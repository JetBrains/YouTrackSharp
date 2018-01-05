using Newtonsoft.Json;

namespace YouTrackSharp.Projects
{
    /// <summary>
    /// Custom field for a project
    /// </summary>
    public class CustomField
    {
        /// <summary>
        /// Creates an instance of the <see cref="CustomField" /> class.
        /// </summary>
        public CustomField()
        {
            Name = string.Empty;
            Url = string.Empty;
            Type = string.Empty;
            CanBeEmpty = false;
            EmptyText = string.Empty;
        }

        /// <summary>
        ///  Name of project custom field.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The Url of the custom field.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Type of this custom field.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        ///  Mandatory binary parameter defining if the field can have empty value or not.
        /// </summary>
        [JsonProperty("canBeEmpty")]
        public bool CanBeEmpty { get; set; }

        /// <summary>
        ///  Text that is shown when the custom field has no value.
        /// </summary>
        [JsonProperty("emptyText")]
        public string EmptyText { get; set; }
    }
}