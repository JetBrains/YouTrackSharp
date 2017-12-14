using Newtonsoft.Json;
using System.Collections.Generic;

namespace YouTrackSharp.AgileBoards
{
    /// <summary>
    /// A class that represents the column settings of a board
    /// </summary>
    public class ColumnSettings
    {
        /// <summary>
        /// Creates an instance of the <see cref="ColumnSettings"/> class
        /// </summary>
        public ColumnSettings()
        {
            VisibleValues = new List<VisibleValue>();
        }

        /// <summary>
        /// Gets or sets the field used to determine the columns of a board
        /// </summary>
        [JsonProperty("field")]
        public Field Field { get; set; }

        /// <summary>
        /// A list of the <see cref="VisibleValues"/> comprising the columns of a board
        /// </summary>
        [JsonProperty("visibleValues")]
        public ICollection<VisibleValue> VisibleValues { get; set; }
    }
}