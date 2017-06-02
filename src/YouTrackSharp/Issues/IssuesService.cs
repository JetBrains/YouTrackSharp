using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Issues-Related-Methods.html">YouTrack Issues Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class IssuesService
    {
        private readonly Connection _connection;

        /// <summary>
        /// Creates an instance of the <see cref="IssuesService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public IssuesService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Get a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-an-Issue.html">Get an Issue</a>.</remarks>
        /// <param name="id">Id of an issue to get.</param>
        /// <param name="wikifyDescription">If set to true, then issue description in the response will be formatted ("wikified"). Defaults to false.</param>
        /// <returns>A <see cref="T:System.Collections.ObjectModel.Collection`1" /> of <see cref="Project" /> that are accessible for currently logged in user. TODO</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Issue> GetIssue(string id, bool wikifyDescription = false)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{id}?wikifyDescription={wikifyDescription}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            
            return JsonConvert.DeserializeObject<Issue>(await response.Content.ReadAsStringAsync());
        }
    }
}