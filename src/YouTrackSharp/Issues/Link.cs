using System;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Represents a <see cref="Link" /> for an <see cref="Issue" />.
    /// </summary>
    public class Link
    {
        /// <summary>
        /// Inward link type, e.g. "is required for".
        /// </summary>
        [JsonProperty("typeInward")]
        public string InwardType { get; set; }
        
        /// <summary>
        /// Outward link type, e.g. "depends on".
        /// </summary>
        [JsonProperty("typeOutward")]
        public string OutwardType { get; set; }
        
        /// <summary>
        /// Type name, e.g. "Depend".
        /// </summary>
        [JsonProperty("typeName")]
        public string TypeName { get; set; }
        
        /// <summary>
        /// Source issue.
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }
        
        /// <summary>
        /// Target issue.
        /// </summary>
        [JsonProperty("target")]
        public string Target { get; set; }
    }
}