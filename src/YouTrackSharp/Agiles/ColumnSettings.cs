using System.Collections.Generic;
using Newtonsoft.Json;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Agile board columns settings.
    /// </summary>
    public class ColumnSettings
    {
        /// <summary>
        /// Id of the ColumnSettings.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Custom field, which values are used for columns. Can be null.
        /// </summary>
        [JsonProperty("field")]
        public CustomField Field { get; set; }

        /// <summary>
        /// Columns that are shown on the board.
        /// </summary>
        [JsonProperty("columns")]
        public List<AgileColumn> Columns { get; set; }
    }
}