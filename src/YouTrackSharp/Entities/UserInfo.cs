namespace YouTrackSharp.Entities
{
    /// <summary>
    /// A class that represents YouTrack user information.
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// Full name of the user.
        /// </summary>
        public string FullName { get; set; }
        
        /// <summary>
        /// Email address of the user.
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Project that was last created.
        /// </summary>
        public string LastCreatedProject { get; set; }
        
        /// <summary>
        /// Project that is filtered on.
        /// </summary>
        public string FilterProject { get; set; }
    }
}