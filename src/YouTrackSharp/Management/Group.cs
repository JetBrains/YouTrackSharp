using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Management
{
    /// <summary>
    /// A class that represents YouTrack group information.
    /// </summary>
    public class Group
    {
        /// <summary>
        /// Creates an instance of the <see cref="User" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="HubApiUser"/> to convert from.</param>
        internal static Group FromApiEntity(HubApiUserGroup entity)
        {
            return new Group() {RingId = entity.Id, Name = entity.Name};
        }
        
        /// <summary>
        /// Id of the group.
        /// </summary>
        [JsonProperty("ringId")]
        public string RingId { get; set; }

        /// <summary>
        /// Name of the group.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}