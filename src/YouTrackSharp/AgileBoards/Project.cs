using Newtonsoft.Json;
using YouTrackSharp.Projects;

namespace YouTrackSharp.AgileBoards
{
    /// <summary>
    /// A class that represents a project in the context of an agile board. The class only provides an id, see <see cref="ProjectsService"/>
    /// for more info on how to access a YouTrack project
    /// </summary>
    public class Project
    {
        /// <summary>
        /// Gets or sets the id of a project
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}