using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouTrackSharp.Projects
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Projects-Related-Methods.html">YouTrack Projects Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class ProjectsService
    {
        private readonly Connection _connection;

        /// <summary>
        /// Creates an instance of the <see cref="ProjectsService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public ProjectsService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Get a list of all accessible projects from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Accessible-Projects.html">Get Accessible Projects</a>.</remarks>
        /// <param name="verbose">If full representation of projects is returned. If this parameter is false, only short names and id's are returned.</param>
        /// <returns>A <see cref="T:System.Collections.ObjectModel.Collection`1" /> of <see cref="Project" /> that are accessible for currently logged in user. TODO</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<Project>> GetAccessibleProjects(bool verbose = false)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/project/all?verbose={verbose}");
            
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<ICollection<Project>>(await response.Content.ReadAsStringAsync());
        }
    }
}