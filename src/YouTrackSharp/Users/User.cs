using Newtonsoft.Json;

namespace YouTrackSharp.Users
{
    /// <summary>
    /// A class that represents YouTrack user information.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Creates an instance of the <see cref="User" /> class based on API response.
        /// </summary>
        public User(Me me)
        {
            Login = me.Login;
            FullName = me.FullName;
            Email = me.Email;
            IsGuest = me.Guest ?? false;
            IsOnline = me.Online ?? false;
            AvatarUrl = me.AvatarUrl;
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