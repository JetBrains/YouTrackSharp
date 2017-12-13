using Newtonsoft.Json;
using System.Collections.Generic;

namespace YouTrackSharp.AgileBoards
{
    public class AgileSettings
    {
        public AgileSettings()
        {
            Projects = new List<Project>();
        }

        [JsonProperty("projects")]
        public ICollection<Project> Projects { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("columnSettings")]
        public ColumnSettings ColumnSettings { get; set; }

        [JsonProperty("timeField")]
        public Field TimeField { get; set; }

        [JsonProperty("projectsColors")]
        public object ProjectsColors { get; set; }

        [JsonProperty("colorConfig")]
        public ColorConfig ColorConfig { get; set; }

        [JsonProperty("completeBacklogHierarhy")]
        public bool CompleteBacklogHierarhy { get; set; }

        public SwimlaneSettings SwimlaneSettings { get; set; }
        public Backlog Backlog { get; set; }
        public ICollection<Sprint> Sprints { get; set; }
    }
}