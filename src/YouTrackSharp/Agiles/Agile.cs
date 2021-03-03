using System.Collections.Generic;
using Newtonsoft.Json;
using YouTrackSharp.Json;
using YouTrackSharp.Management;
using YouTrackSharp.Projects;
using YouTrackSharp.SerializationAttributes;

namespace YouTrackSharp.Agiles {
  /// <summary>
  /// Represents an agile board configuration.
  /// </summary>
  public class Agile {
    /// <summary>
    /// Id of the Agile.
    /// </summary>
    [JsonProperty("id")]
    public string Id { get; set; }

    /// <summary>
    /// The name of the agile board. Can be null.
    /// </summary>
    [JsonProperty("name")]
    public string Name { get; set; }

    /// <summary>
    /// Owner of the agile board. Can be null.
    /// </summary>
    [JsonProperty("owner")]
    public User Owner { get; set; }

    /// <summary>
    /// The user group that can view this board. Can be null.
    /// </summary>
    [Verbose]
    [JsonProperty("visibleFor")]
    public Group VisibleFor { get; set; }

    /// <summary>
    /// When true, the board is visible to everyone who can view all projects that are associated with the board.
    /// </summary>
    [Verbose]
    [JsonProperty("visibleForProjectBased")]
    public bool VisibleForProjectBased { get; set; }

    /// <summary>
    /// Group of users who can update board settings. Can be null.
    /// </summary>
    [Verbose]
    [JsonProperty("updateableBy")]
    public Group UpdateableBy { get; set; }

    /// <summary>
    /// When true, anyone who can update the associated projects can update the board.
    /// </summary>
    [Verbose]
    [JsonProperty("updateableByProjectBased")]
    public bool UpdateableByProjectBased { get; set; }

    /// <summary>
    /// When true, the orphan swimlane is placed at the top of the board. Otherwise, the orphans swimlane is located
    /// below all other swimlanes.
    /// </summary>
    [Verbose]
    [JsonProperty("orphansAtTheTop")]
    public bool OrphansAtTheTop { get; set; }

    /// <summary>
    /// When true, the orphans swimlane is not displayed on the board.
    /// </summary>
    [Verbose]
    [JsonProperty("hideOrphansSwimlane")]
    public bool HideOrphansSwimlane { get; set; }

    /// <summary>
    /// A custom field that is used as the estimation field for the board. Can be null.
    /// </summary>
    [Verbose]
    [JsonProperty("estimationField")]
    public CustomField EstimationField { get; set; }

    /// <summary>
    /// A custom field that is used as the original estimation field for the board. Can be null.
    /// </summary>
    [Verbose]
    [JsonProperty("originalEstimationField")]
    public CustomField OriginalEstimationField { get; set; }

    /// <summary>
    /// A collection of projects associated with the board.
    /// </summary>
    [Verbose]
    [JsonProperty("projects")]
    public List<Project> Projects { get; set; }

    /// <summary>
    /// The set of sprints that are associated with the board.
    /// </summary>
    [Verbose]
    [JsonProperty("sprints")]
    public List<Sprint> Sprints { get; set; }

    /// <summary>
    /// A sprint that is actual for the current date. Read-only. Can be null.
    /// </summary>
    [Verbose]
    [JsonProperty("currentSprint")]
    public Sprint CurrentSprint { get; set; }

    /// <summary>
    /// Column settings of the board. Read-only.
    /// </summary>
    [Verbose]
    [JsonProperty("columnSettings")]
    public ColumnSettings ColumnSettings { get; set; }

    /// <summary>
    /// Settings of the board swimlanes. Can be null.
    /// </summary>
    [Verbose]
    [JsonProperty("swimlaneSettings")]
    [JsonConverter(typeof(KnownTypeConverter<SwimlaneSettings>))]
    public SwimlaneSettings SwimlaneSettings { get; set; }

    /// <summary>
    /// Settings of the board sprints. Read-only.
    /// </summary>
    [Verbose]
    [JsonProperty("sprintsSettings")]
    public SprintsSettings SprintsSettings { get; set; }

    /// <summary>
    /// Color coding settings for the board. Can be null.
    /// </summary>
    [Verbose]
    [JsonProperty("colorCoding")]
    [JsonConverter(typeof(KnownTypeConverter<ColorCoding>))]
    public ColorCoding ColorCoding { get; set; }

    /// <summary>
    /// Status of the board. Read-only.
    /// </summary>
    [Verbose]
    [JsonProperty("status")]
    public AgileStatus Status { get; set; }
  }
}
