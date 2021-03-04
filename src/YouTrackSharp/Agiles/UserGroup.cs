using Newtonsoft.Json;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represents a group of users.
  /// </summary>
  public class UserGroup {
    /// <summary>
    /// Id of the UserGroup.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }
    
    /// <summary>
    /// The name of the group. Read-only. Can be null. 
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
    
    /// <summary>
    /// ID of the group in Hub. Use this ID for operations in Hub, and for matching groups between YouTrack and Hub. Read-only. Can be null.  
    /// </summary>
    [JsonProperty("ringId")]
    public string RingId { get; set; }
    
    /// <summary>
    /// The number of users in the group. Read-only. 
    /// </summary>
    [JsonProperty("userCount")]
    public long UserCount { get; set; }
    
    /// <summary>
    /// The URL of the group icon. Read-only. Can be null. 
    /// </summary>
    [JsonProperty("icon")]
    public string Icon { get; set; }
    
    /// <summary>
    /// True if this group contains all users, otherwise false. Read-only. 
    /// </summary>
    [JsonProperty("allUsersGroup")]
    public bool AllUsersGroup { get; set; }
    
    /// <summary>
    /// Project that has this group set as a team. Returns null, if there is no such project. Read-only. Can be null. 
    /// </summary>
    [JsonProperty("teamForProject")]
    public Project TeamForProject { get; set; }
  }
}