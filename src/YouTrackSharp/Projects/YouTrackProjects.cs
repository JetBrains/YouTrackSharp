using System;
using System.Collections.Generic;
using YouTrackSharp.DataModel;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Projects
{
    public class YouTrackProjects
    {
        private readonly YouTrackServer _youTrackServer;

        public YouTrackProjects(YouTrackServer youTrackServer)
        {
            _youTrackServer = youTrackServer;
        }

        /// <summary>
        /// Returns general information for given project
        /// </summary>
        /// <param name="projectKey">Project short name</param>
        /// <returns>General project information</returns>
        public YouTrackProjectInformation GetProjectInformation(string projectKey)
        {
            return XmlConverter.ExtractProjectInformation(_youTrackServer.HttpGet(String.Format("project/byname/{0}", projectKey), "GetProjectInformation"));
        }

        /// <summary>
        /// Returns the list of available issue states
        /// </summary>
        /// <returns>List of states</returns>
        public IEnumerable<YouTrackState> GetStates()
        {
            return XmlConverter.ExtractProperties<YouTrackState>(_youTrackServer.HttpGet("project/states", "GetStates"), "state", "name");
        }

        /// <summary>
        /// Returns the list of states that are considered as resolved
        /// </summary>
        /// <returns>List of resolutions</returns>
        public IEnumerable<YouTrackResolution> GetResolutions()
        {
            return XmlConverter.ExtractProperties<YouTrackResolution>(_youTrackServer.HttpGet("project/resolutions", "GetResolutions"), "resolution", "name");
        }

        /// <summary>
        /// Returns the list of available priorities
        /// </summary>
        /// <returns>List of priorities</returns>
        public IEnumerable<YouTrackPriority> GetPriorities()
        {
            return XmlConverter.ExtractProperties<YouTrackPriority>(_youTrackServer.HttpGet("project/priorities", "GetPriorities"), "priority", "name");
        }

        /// <summary>
        /// Returns the list of available issue types
        /// </summary>
        /// <returns>List of types</returns>
        public IEnumerable<YouTrackIssueType> GetIssueTypes()
        {
            return XmlConverter.ExtractProperties<YouTrackIssueType>(_youTrackServer.HttpGet("project/types", "GetIssueTypes"), "type", "name");
        }
    }
}