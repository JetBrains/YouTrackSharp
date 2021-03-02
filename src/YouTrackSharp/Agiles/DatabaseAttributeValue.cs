using Newtonsoft.Json;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represents string reference to the value.
  /// </summary>
  public class DatabaseAttributeValue {
    /// <summary>
    /// Id of the DatabaseAttributeValue.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }
  }
}
