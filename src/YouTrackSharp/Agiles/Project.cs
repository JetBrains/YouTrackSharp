using Newtonsoft.Json;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represents a project in the context of an agile board. The class only provides the id, short name and name of the
  /// project see <see cref="ProjectService"/> for more info on how to access a YouTrack project.
  /// </summary>
  public class Project {
    /// <summary>
    /// Id of the Project.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// The ID of the project. This short name is also a prefix for an issue ID. Can be null.
    /// </summary>
    [JsonProperty("shortName")]
    public string ShortName { get; set; }

    /// <summary>
    /// The name of the issue folder. Can be null.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
  }
}
