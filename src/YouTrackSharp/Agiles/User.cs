using Newtonsoft.Json;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Represents a user in the context of an agile board. The class only provides the id, ringId (hub ID) and full name
    /// of the user, see <see cref="ManagementService"/> for more info on how to access a YouTrack user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Id of the User.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// ID of the user in Hub. You can use this ID for operations in Hub, and for matching users between YouTrack and
        /// Hub. Read-only. Can be null.
        /// </summary>
        [JsonProperty("ringId")]
        public string RingId { get; set; }

        /// <summary>
        /// Full name of the user.
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }
    }
}