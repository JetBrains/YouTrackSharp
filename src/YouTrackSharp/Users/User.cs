using Newtonsoft.Json;

namespace YouTrackSharp.Users
{
    /// <summary>
    /// A class that represents YouTrack user information.
    /// </summary>
    public class User
    {
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
        /// Project that was last created.
        /// </summary>
        [JsonProperty("lastCreatedProject")]
        public string LastCreatedProject { get; set; }
        
        /// <summary>
        /// Project that is filtered on.
        /// </summary>
        [JsonProperty("filterProject")]
        public string FilterProject { get; set; }
    }
}