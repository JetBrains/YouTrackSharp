using System.Collections.Generic;
using Newtonsoft.Json;
using YouTrackSharp.Json;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Settings of swimlanes that are identified by the set of values in the selected field. For example, you can set
    /// swimlanes to represent issues for each Assignee.
    /// </summary>
    public class AttributeBasedSwimlaneSettings : SwimlaneSettings
    {
        /// <summary>
        /// CustomField which values are used to identify swimlane.
        /// </summary>
        [JsonProperty("field")]
        [JsonConverter(typeof(KnownTypeConverter<FilterField>))]
        public FilterField Field { get; set; }

        /// <summary>
        /// Swimlanes that are visible on the Board.
        /// </summary>
        [JsonProperty("values")]
        public List<SwimlaneEntityAttributeValue> Values { get; set; }
    }
}