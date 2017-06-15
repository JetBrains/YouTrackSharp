using System;
using System.Collections.Generic;
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
        /// <returns>The <see cref="Issue" /> that matches the requested <paramref name="id"/>.</returns>
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

        /// <summary>
        /// Get issues in a project from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Issues-in-a-Project.html">Get Issues in a Project</a>.</remarks>
        /// <param name="projectId">Id of a project to get issues from.</param>
        /// <param name="filter">Apply a filter to issues in a project.</param>
        /// <param name="skip">The number of issues to skip before getting a list of issues.</param>
        /// <param name="take">Maximum number of issues to be returned. Defaults to the server-side default of the YouTrack server instance..</param>
        /// <param name="updatedAfter">Only issues updated after the specified date will be retrieved.</param>
        /// <param name="wikifyDescription">If set to true, then issue description in the response will be formatted ("wikified"). Defaults to false.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<Issue>> GetIssuesInProject(string projectId, string filter = null, int? skip = null, int? take = null, DateTime? updatedAfter = null, bool wikifyDescription = false)
        {
            var queryString = new List<string>(6);
            if (!string.IsNullOrEmpty(filter))
            {
                queryString.Add($"filter={WebUtility.UrlEncode(filter)}");
            }
            if (skip.HasValue)
            {
                queryString.Add($"after={skip}");
            }
            if (take.HasValue)
            {
                queryString.Add($"max={take}");
            }
            if (updatedAfter.HasValue)
            {
                var offset = new DateTimeOffset(updatedAfter.Value);
                queryString.Add($"updatedAfter={offset.ToUnixTimeMilliseconds()}");
            }
            
            queryString.Add($"wikifyDescription={wikifyDescription}");

            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/byproject/{projectId}?{query}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            
            return JsonConvert.DeserializeObject<ICollection<Issue>>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Get issues from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-the-List-of-Issues.html">Get the List of Issues</a>.</remarks>
        /// <param name="filter">Apply a filter to issues.</param>
        /// <param name="skip">The number of issues to skip before getting a list of issues.</param>
        /// <param name="take">Maximum number of issues to be returned. Defaults to the server-side default of the YouTrack server instance..</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<Issue>> GetIssues(string filter = null, int? skip = null, int? take = null)
        {
            var queryString = new List<string>(6);
            if (!string.IsNullOrEmpty(filter))
            {
                queryString.Add($"filter={WebUtility.UrlEncode(filter)}");
            }
            if (skip.HasValue)
            {
                queryString.Add($"after={skip}");
            }
            if (take.HasValue)
            {
                queryString.Add($"max={take}");
            }
            
            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue?{query}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var wrapper = JsonConvert.DeserializeObject<IssueCollectionWrapper>(
                await response.Content.ReadAsStringAsync());
            return wrapper.Issues;
        }
    }
}