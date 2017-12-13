using Newtonsoft.Json;
using System.Collections.Generic;

namespace YouTrackSharp.AgileBoards
{
    public class AgileSettings
    {
        /// <summary>
        /// Creates an instance of the <see cref="AgileSettings" /> class.
        /// </summary>
        public AgileSettings()
        {
            Projects = new List<Project>();
            Sprints = new List<Sprint>();
        }

        /// <summary>
        /// List of  <see cref="Project"/>s included on the board
        /// </summary>
        [JsonProperty("projects")]
        public ICollection<Project> Projects { get; set; }

        /// <summary>
        /// The name of the agile board
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// The internal id used by YouTrack to identify the board
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// The filter query used by the board
        /// </summary>
        [JsonProperty("query")]
        public string Query { get; set; }

        /// <summary>
        /// The <see cref="ColumnSettings"/> used by the board
        /// </summary>
        [JsonProperty("columnSettings")]
        public ColumnSettings ColumnSettings { get; set; }

        /// <summary>
        /// If time tracking is enabled this property returns the estimation field
        /// </summary>
        [JsonProperty("timeField")]
        public Field TimeField { get; set; }

        /// <summary>
        /// If Cards are color coded by a field this field can be found here
        /// </summary>
        [JsonProperty("colorConfig")]
        public ColorConfig ColorConfig { get; set; }

        /// <summary>
        /// I have no idea what this field means.
        /// </summary>
        [JsonProperty("completeBacklogHierarhy")]
        public bool CompleteBacklogHierarhy { get; set; }

        /// <summary>
        /// The <see cref="SwimlaneSettings"/> of the board
        /// </summary>
        public SwimlaneSettings SwimlaneSettings { get; set; }

        /// <summary>
        /// The saved search that returns the backlog for the board
        /// <remarks>See <see cref="Backlog"/> for more info</remarks>
        /// </summary>
        public Backlog Backlog { get; set; }

        /// <summary>
        /// The list of <see cref="Sprint"/>s associated with the board
        /// </summary>
        public ICollection<Sprint> Sprints { get; set; }
    }
}