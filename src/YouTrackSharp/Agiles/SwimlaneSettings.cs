using Newtonsoft.Json;
using YouTrackSharp.SerializationAttributes;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Base entity for different swimlane settings
  /// </summary>
  [KnownType(typeof(AttributeBasedSwimlaneSettings))]
  [KnownType(typeof(IssueBasedSwimlaneSettings))]
  public class SwimlaneSettings {
    /// <summary>
    /// Id of the SwimlaneSettings.
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
