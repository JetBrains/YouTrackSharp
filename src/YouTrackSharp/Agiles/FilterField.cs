using Newtonsoft.Json;
using YouTrackSharp.SerializationAttributes;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represents an issue property, which can be a predefined field, a custom field, a link, and so on.
  /// </summary>
  [KnownType(typeof(PredefinedFilterField))]
  [KnownType(typeof(CustomFilterField))]
  public class FilterField {
    /// <summary>
    /// Id of the FilterField.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Presentation of the field. Read-only.
    /// </summary>
    [JsonProperty("presentation")]
    public string Presentation { get; set; }

    /// <summary>
    /// The name of the field. Read-only.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }
  }
}
