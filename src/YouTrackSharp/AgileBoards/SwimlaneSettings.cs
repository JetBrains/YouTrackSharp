using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrackSharp.AgileBoards
{
    /// <summary>
    /// A class that represents the swimlane settings of a board
    /// </summary>
    public class SwimlaneSettings
    {
        /// <summary>
        /// Creates a new <see cref="SwimlaneSettings"/> object
        /// </summary>
        public SwimlaneSettings()
        {
            Values = new List<string>();
        }

        /// <summary>
        /// Gets or sets the type used to determine how to represent swimlanes on a board
        /// </summary>
        [JsonProperty("field")]
        public Field Field { get; set; }

        /// <summary>
        /// Gets or sets the default card type of a board
        /// </summary>
        [JsonProperty("defaultCardType")]
        public string DefaultCardType { get; set; }

        /// <summary>
        /// Gets an <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="T:System.String"/> of the values used to represent swimlanes of a board
        /// </summary>
        [JsonProperty("values")]
        public ICollection<string> Values { get; set; }
    }
}