using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Wrapper for <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Change" />.
    /// </summary>
    internal class ChangeCollectionWrapper
    {
        /// <summary>
        /// Wrapped <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Change" />.
        /// </summary>
        [JsonProperty("change")]
        public ICollection<Change> Changes { get; set; }
    }
}