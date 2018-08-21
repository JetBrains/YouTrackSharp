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
    public class ProjectsService : IProjectsService
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

        /// <inheritdoc />
        public async Task<ICollection<Project>> GetAccessibleProjects(bool verbose = false)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/project/all?verbose={verbose}");
            
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<ICollection<Project>>(await response.Content.ReadAsStringAsync());
        }
    }
}