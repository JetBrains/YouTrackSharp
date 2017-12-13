using Newtonsoft.Json;
using System.Collections.Generic;

namespace YouTrackSharp.AgileBoards
{
    public class ColumnSettings
    {
        [JsonProperty("field")]
        public Field Field { get; set; }

        [JsonProperty("visibleValues")]
        public ICollection<VisibleValue> VisibleValues { get; set; }
    }
}