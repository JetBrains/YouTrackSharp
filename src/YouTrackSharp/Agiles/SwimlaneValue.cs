using Newtonsoft.Json;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represents single swimlane in case of IssueBasedSwimlaneSettings.
  /// </summary>
  public class SwimlaneValue {
    /// <summary>
    /// Id of the SwimlaneValue.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Name of a value. Read-only. Can be null.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
  }
}
