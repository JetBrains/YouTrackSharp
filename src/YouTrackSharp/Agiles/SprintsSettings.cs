using Newtonsoft.Json;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Describes sprints configuration.
    /// </summary>
    public class SprintsSettings
    {
        /// <summary>
        /// Id of the SprintsSettings.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// If true, issues should be added to the board manually. If false, issues are shown on the board based on the
        /// query and/or value of a field.
        /// </summary>
        [JsonProperty("isExplicit")]
        public bool IsExplicit { get; set; }

        /// <summary>
        /// If true, cards can be present on several sprints of this board.
        /// </summary>
        [JsonProperty("cardOnSeveralSprints")]
        public bool CardOnSeveralSprints { get; set; }

        /// <summary>
        /// New cards are added to this sprint by default. This setting applies only if isExplicit == true. Can be null.
        /// </summary>
        [JsonProperty("defaultSprint")]
        public Sprint DefaultSprint { get; set; }

        /// <summary>
        /// If true, agile board has no distinct sprints in UI. However, in API it will look like it has only one active
        /// (not-archived) sprint.
        /// </summary>
        [JsonProperty("disableSprints")]
        public bool DisableSprints { get; set; }

        /// <summary>
        /// Issues that match this query will appear on the board. This setting applies only if isExplicit == false. Can be
        /// null.
        /// </summary>
        [JsonProperty("explicitQuery")]
        public string ExplicitQuery { get; set; }

        /// <summary>
        /// Based on the value of this field, issues will be assigned to the sprints. This setting applies only if
        /// isExplicit == false. Can be null.
        /// </summary>
        [JsonProperty("sprintSyncField")]
        public CustomField SprintSyncField { get; set; }

        /// <summary>
        /// If true, subtasks of the cards, that are present on the board, will be hidden if they match board query. This
        /// setting applies only if isExplicit == false.
        /// </summary>
        [JsonProperty("hideSubtasksOfCards")]
        public bool HideSubtasksOfCards { get; set; }
    }
}