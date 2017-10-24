using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// A class that represents YouTrack issue Changeset.
    /// </summary>
    public class Changeset
    {
        /// <summary>
        /// Creates an instance of the <see cref="Changeset" /> class.
        /// </summary>
        public Changeset()
        {
            Changes = new List<Change>();
        }

        /// <summary>
        /// Id of the change.
        /// </summary>
        [JsonProperty("issue")]
        public Issue Issue { get; set; }

        /// <summary>
        /// Fields.
        /// </summary>
        [JsonProperty("change")]
        public ICollection<Change> Changes { get; set; }
    }
}