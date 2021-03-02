using Newtonsoft.Json;

namespace YouTrackSharp.Agiles {
  
  /// <summary>
  /// Describe rules according to which different colors are used for cards on agile board.
  /// </summary>
  public class ColorCoding {
    /// <summary>
    /// Id of the ColorCoding.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }
  }
}
