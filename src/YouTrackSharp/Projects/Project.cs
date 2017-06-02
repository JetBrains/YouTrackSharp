using System.Collections.Generic;
using Newtonsoft.Json;
using YouTrackSharp.Json;

namespace YouTrackSharp.Projects
{
    /// <summary>
    /// A class that represents YouTrack project information.
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Creates an instance of the <see cref="Project" /> class.
        /// </summary>
        public Project()
        {
            Versions = new List<string>();
            Subsystems = new List<SubValue>();
            AssigneesLogin = new List<SubValue>();
            AssigneesFullName = new List<SubValue>();
        }
        
        /// <summary>
        /// Name of the project.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
        
        /// <summary>
        /// Short name of the project.
        /// </summary>
        [JsonProperty("shortName")]
        public string ShortName { get; set; }
        
        /// <summary>
        /// Description of the project.
        /// </summary>
        /// <remarks>
        /// Only available when verbose project information is retrieved.
        /// </remarks>
        [JsonProperty("description")]
        public string Description { get; set; }
        
        /// <summary>
        /// Versions defined for the project.
        /// </summary>
        /// <remarks>
        /// Only available when verbose project information is retrieved.
        /// </remarks>
        [JsonConverter(typeof(ProjectVersionConverter))]
        [JsonProperty("versions")]
        public ICollection<string> Versions { get; set; }
        
        /// <summary>
        /// Subsystems defined for the project.
        /// </summary>
        /// <remarks>
        /// Only available when verbose project information is retrieved.
        /// </remarks>
        [JsonProperty("subsystems")]
        public ICollection<SubValue> Subsystems { get; set; }
        
        /// <summary>
        /// Available assignee login names defined for the project.
        /// </summary>
        /// <remarks>
        /// Only available when verbose project information is retrieved.
        /// </remarks>
        [JsonProperty("assigneesLogin")]
        public ICollection<SubValue> AssigneesLogin { get; set; }
        
        /// <summary>
        /// Available assignee full names defined for the project.
        /// </summary>
        /// <remarks>
        /// Only available when verbose project information is retrieved.
        /// </remarks>
        [JsonProperty("assigneesFullName")]
        public ICollection<SubValue> AssigneesFullName { get; set; }
        
        /// <summary>
        /// A boolean that is true when the project is importing, otherwise false.
        /// </summary>
        /// <remarks>
        /// Only available when verbose project information is retrieved.
        /// </remarks>
        [JsonProperty("isImporting")]
        public string IsImporting { get; set; }
    }
}