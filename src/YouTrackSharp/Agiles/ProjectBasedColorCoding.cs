using System.Collections.Generic;
using Newtonsoft.Json;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Allows to set card's color based on it's project
    /// </summary>
    public class ProjectBasedColorCoding : ColorCoding
    {
        /// <summary>
        /// Collection of per-project color settings
        /// </summary>
        [JsonProperty("projectColors")]
        public List<ProjectColor> ProjectColors { get; set; }
    }
}