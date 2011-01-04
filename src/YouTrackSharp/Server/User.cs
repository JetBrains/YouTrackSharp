using JsonFx.Json;

namespace YouTrackSharp.Server
{
    public class User
    {
        [JsonIgnore]
        public string Username { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string LastCreatedProject { get; set; }
    }
}