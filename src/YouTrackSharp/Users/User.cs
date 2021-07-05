using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Users
{
    /// <summary>
    /// A class that represents YouTrack user information.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Creates an instance of the <see cref="User" /> class from api client entity.
        /// </summary>
        /// <param name="entity">Api client entity of type <see cref="Me"/> to convert from.</param>
        public static User FromApiEntity(Me entity)
        {
            return new User()
            {
                Login = entity.Login,
                FullName = entity.FullName,
                Email = entity.Email,
                IsGuest = entity.Guest ?? true,
                IsOnline = entity.Online ?? false,
                AvatarUrl = entity.AvatarUrl
            };
        }
        
        /// <summary>
        /// Login of the user.
        /// </summary>
        [JsonProperty("login")]
        public string Login { get; set; }
        
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
        /// Is a guest?
        /// </summary>
        [JsonProperty("guest")]
        public bool IsGuest { get; set; }
        
        /// <summary>
        /// Is online?
        /// </summary>
        [JsonProperty("online")]
        public bool IsOnline { get; set; }
        
        /// <summary>
        /// Avatar URL.
        /// </summary>
        [JsonProperty("avatarUrl")]
        public string AvatarUrl { get; set; }
    }
}