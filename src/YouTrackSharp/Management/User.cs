using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Management
{
    /// <summary>
    /// A class that represents YouTrack user information.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Creates an instance of the <see cref="User" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="HubApiUser"/> to convert from.</param>
        internal static User FromApiEntity(HubApiUser entity)
        {
            return new User()
            {
                RingId = entity.Id,
                Username = entity.Login,
                FullName = entity.Name,
                Email = entity.Profile?.Email?.Email,
                Jabber = entity.Profile?.Jabber?.Jabber,
                Banned = entity.Banned ?? false,
                BanBadge = entity.BanBadge,
                BanReason = entity.BanReason
            };
        }
        
        /// <summary>
        /// Ring ID of the user.
        /// </summary>
        [JsonProperty("ringId")]
        public string RingId { get; set; }
        /// <summary>
        /// Username of the user.
        /// </summary>
        [JsonProperty("login")]
        public string Username { get; set; }
        
        /// <summary>
        /// Full name of the user.
        /// </summary>
        [JsonProperty("fullName")]
        public string FullName { get; set; }
        
        /// <summary>
        /// Email address of the user.
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }
        
        /// <summary>
        /// Jabber of the user.
        /// </summary>
        [JsonProperty("jabber")]
        public string Jabber { get; set; }
        
        /// <summary>
        /// Jabber of the user.
        /// </summary>
        [JsonProperty("banned")]
        public bool Banned { get; set; }
        
        /// <summary>
        /// Jabber of the user.
        /// </summary>
        [JsonProperty("banBadge")]
        public string BanBadge { get; set; }
        
        /// <summary>
        /// Jabber of the user.
        /// </summary>
        [JsonProperty("banReason")]
        public string BanReason { get; set; }
    }
}