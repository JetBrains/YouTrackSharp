using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using YouTrackSharp.Generated;
using YouTrackSharp.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Represents an <see cref="Attachment" /> for an <see cref="Issue" />.
    /// </summary>
    public class Attachment
    {
        /// <summary>
        /// Creates an instance of the <see cref="Attachment" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="IssueAttachment"/> to convert from.</param>
        internal static Attachment FromApiEntity(IssueAttachment entity)
        {
            return new Attachment()
            {
                Id = entity.Id,
                Url = new Uri(entity.Url.TrimStart('/'), UriKind.RelativeOrAbsolute),
                Name = entity.Name,
                Author = entity.Author.Login,
                Group = entity.Visibility?.ToSinglePermittedGroup(),
                Created = new DateTime(entity.Created ?? 0)
            };
        }
        
        /// <summary>
        /// Id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
        
        /// <summary>
        /// Url.
        /// </summary>
        [JsonProperty("url")]
        public Uri Url { get; set; }
        
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