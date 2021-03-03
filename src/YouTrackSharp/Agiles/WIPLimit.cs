using Newtonsoft.Json;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represents WIP limits for particular column. If they are not satisfied, the column will be highlighted in UI.
  /// </summary>
  public class WIPLimit {
    /// <summary>
    /// Id of the WIPLimit.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Maximum number of cards in column. Can be null.
    /// </summary>
    [JsonProperty("max")]
    public int? Max { get; set; }

    /// <summary>
    /// Minimum number of cards in column. Can be null.
    /// </summary>
    [JsonProperty("min")]
    public int? Min { get; set; }
  }
}
