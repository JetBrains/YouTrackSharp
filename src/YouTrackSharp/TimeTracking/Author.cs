using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.TimeTracking
{
    /// <summary>
    /// A class that represents YouTrack issue work item author information.
    /// </summary>
    public class Author
    {
        /// <summary>
        /// Creates an instance of the <see cref="Author" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="Me"/> to convert from.</param>
        internal static Author FromApiEntity(Me entity)
        {
            return new Author() {Login = entity.Login};
        }

        /// <summary>
        /// Converts to instance of the <see cref="Me" /> class used in api client.
        /// </summary>
        internal Me ToApiEntity()
        {
            return new Me() {Login = Login};
        }

        /// <summary>
        /// Login.
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; }
    }
}