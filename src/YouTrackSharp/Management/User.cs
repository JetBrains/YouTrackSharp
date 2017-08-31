using Newtonsoft.Json;

namespace YouTrackSharp.Management
{
    /// <summary>
    /// A class that represents YouTrack user information.
    /// </summary>
    public class User
    {
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
    }
}