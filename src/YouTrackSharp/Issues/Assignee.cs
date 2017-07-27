using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Represents an <see cref="Assignee" /> for an <see cref="Issue" />.
    /// </summary>
    public class Assignee
    {
        /// <summary>
        /// Username.
        /// </summary>
        [JsonProperty("value")]
        public string UserName { get; set; }
        
        /// <summary>
        /// Full name.
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }
}