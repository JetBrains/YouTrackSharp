using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Wrapper for <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" />.
    /// </summary>
    internal class IssueCollectionWrapper
    {
        /// <summary>
        /// Wrapped <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" />.
        /// </summary>
        [JsonProperty("issue")]
        public ICollection<Issue> Issues { get; set; }
    }
}