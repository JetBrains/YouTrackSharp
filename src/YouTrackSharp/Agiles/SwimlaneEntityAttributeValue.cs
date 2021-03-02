using Newtonsoft.Json;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represents a single swimlane in case of AttributeBasedSwimlaneSettings.
  /// </summary>
  public class SwimlaneEntityAttributeValue : DatabaseAttributeValue {
    /// <summary>
    /// Name of the swimlane. Can be null.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// If true, issues in this swimlane are considered to be resolved. Can be updated only for newly created value.
    /// Read-only.
    /// </summary>
    [JsonProperty("isResolved")]
    public bool IsResolved { get; set; }
  }
}
