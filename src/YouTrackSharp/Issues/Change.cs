using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// A class that represents YouTrack issue change.
    /// </summary>
    public class Change
    {
        /// <summary>
        /// Creates an instance of the <see cref="Change" /> class.
        /// </summary>
        public Change()
        {
            Fields = new List<Field>();
        }

        /// <summary>
        /// Id of the change.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Fields.
        /// </summary>
        [JsonProperty("field")]
        public ICollection<Field> Fields { get; set; }
    }
}