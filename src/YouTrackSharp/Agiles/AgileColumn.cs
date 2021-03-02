using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represents settings for a single board column
  /// </summary>
  public class AgileColumn {
    /// <summary>
    /// Id of the AgileColumn.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// Text presentation of values stored in a column. Read-only. Can be null.
    /// </summary>
    [JsonProperty("presentation")]
    public string Presentation { get; set; }

    /// <summary>
    /// true if a column represents resolved state of an issue. Can be updated only for newly created value. Read-only.
    /// </summary>
    [JsonProperty("isResolved")]
    public bool IsResolved { get; set; }

    /// <summary>
    /// Order of this column on board, counting from left to right.
    /// </summary>
    [JsonProperty("ordinal")]
    public int Ordinal { get; set; }

    /// <summary>
    /// WIP limit for this column. Can be null.
    /// </summary>
    [JsonProperty("wipLimit")]
    public WIPLimit WipLimit { get; set; }

    /// <summary>
    /// Field values represented by this column.
    /// </summary>
    [JsonProperty("fieldValues")]
    public List<AgileColumnFieldValue> FieldValues { get; set; }
  }
}
