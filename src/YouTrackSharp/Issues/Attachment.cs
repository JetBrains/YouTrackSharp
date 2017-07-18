using System;
using Newtonsoft.Json;
using YouTrackSharp.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Represents an <see cref="Attachment" /> for an <see cref="Issue" />.
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// Url.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
        
        /// <summary>
        /// Name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Author.
        /// </summary>
        [JsonProperty("authorLogin")]
        public string Author { get; set; }
        
        /// <summary>
        /// Group.
        /// </summary>
        [JsonProperty("group")]
        public string Group { get; set; }
        
        /// <summary>
        /// Created.
        /// </summary>
        [JsonProperty("created")]
        [JsonConverter(typeof(UnixDateTimeOffsetConverter))]
        public DateTime Created { get; set; }
    }
}