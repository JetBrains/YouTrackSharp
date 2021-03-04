using System.Collections.Generic;
using Newtonsoft.Json;
using YouTrackSharp.Json;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Base entity for different swimlane settings
    /// </summary>
    public class IssueBasedSwimlaneSettings : SwimlaneSettings
    {
        /// <summary>
        /// CustomField which values are used to identify swimlane.
        /// </summary>
        [JsonProperty("field")]
        [JsonConverter(typeof(KnownTypeConverter<FilterField>))]
        public FilterField Field { get; set; }

        /// <summary>
        /// Value of a field that a card would have by default. Can be null.
        /// </summary>
        [JsonProperty("defaultCardType")]
        public SwimlaneValue DefaultCardType { get; set; }

        /// <summary>
        /// When issue has one of this values, it becomes a swimlane on this board.
        /// </summary>
        [JsonProperty("values")]
        public List<SwimlaneValue> Values { get; set; }
    }
}