using Newtonsoft.Json;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represent color setting for one project on the board.
  /// </summary>
  public class ProjectColor {
    /// <summary>
    /// Id of the ProjectColor.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// A project, to which color this setting describes. Read-only. Can be null.
    /// </summary>
    [JsonProperty("project")]
    public Project Project { get; set; }

    /// <summary>
    /// A color, that issues of this project will have on the board. Read-only. Can be null.
    /// </summary>
    [JsonProperty("color")]
    public FieldStyle Color { get; set; }
  }
}
