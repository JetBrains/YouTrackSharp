using System;
using System.Collections.Generic;
using YouTrackSharp.DataModel;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Administration
{
    public class YouTrackAdministration
    {
        private readonly YouTrackServer _youTrackServer;

        public YouTrackAdministration(YouTrackServer youTrackServer)
        {
            _youTrackServer = youTrackServer;
        }

        /// <summary>
        /// Returns the TeamCity builds associated with given project
        /// </summary>
        /// <param name="projectKey">Project short name</param>
        /// <returns>List of builds</returns>
        public IEnumerable<YouTrackProjectBuild> GetBuilds(string projectKey)
        {
            string getBuildsUri = String.Format("admin/project/{0}/build", projectKey);
            return XmlConverter.ExtractBuilds(_youTrackServer.HttpGet(getBuildsUri, "GetBuilds"));
        }

    }
}