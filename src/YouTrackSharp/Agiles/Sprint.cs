using Newtonsoft.Json;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represents a sprint in the context of an agile board. The class only provides the sprint's id and name, see <see
  /// cref="SprintService"/> for more info on how to access a YouTrack sprint.
  /// </summary>
  public class Sprint {
    /// <summary>
    /// Id of the Sprint.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Name of the sprint. Can be null.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
  }
}
