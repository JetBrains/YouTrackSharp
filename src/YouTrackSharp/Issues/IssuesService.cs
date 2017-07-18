using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="id"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Issue> GetIssue(string id, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            
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
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<Issue>> GetIssuesInProject(string projectId, string filter = null, int? skip = null, int? take = null, DateTime? updatedAfter = null, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            
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

            var wrapper = JsonConvert.DeserializeObject<IssueCollectionWrapper>(await response.Content.ReadAsStringAsync());
            return wrapper.Issues;
        }

        /// <summary>
        /// Creates an issue on the server in a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Create-New-Issue.html">Create New Issue</a>.</remarks>
        /// <param name="projectId">Id of the project to create an issue in.</param>
        /// <param name="issue">The <see cref="Issue" /> to create. At the minimum needs the Summary field populated.</param>
        /// <returns>The newly created <see cref="Issue" />'s id on the server.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<string> CreateIssue(string projectId, Issue issue)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            
            var queryString = new List<string>(3);
            queryString.Add($"project={projectId}");
            
            if (!string.IsNullOrEmpty(issue.Summary))
            {
                queryString.Add($"summary={WebUtility.UrlEncode(issue.Summary)}");
            }
            if (!string.IsNullOrEmpty(issue.Description))
            {
                queryString.Add($"description={WebUtility.UrlEncode(issue.Description)}");
            }
            
            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PutAsync($"rest/issue?{query}", new MultipartFormDataContent());

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            
            // Extract issue id from Location header response
            var marker = "rest/issue/";
            var locationHeader = response.Headers.Location.ToString();
            
            return locationHeader.Substring(locationHeader.IndexOf(marker, StringComparison.OrdinalIgnoreCase) + marker.Length);
        }

        /// <summary>
        /// Applies a command to a specific issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Apply-Command-to-an-Issue.html">Apply Command to an Issue</a>.</remarks>
        /// <param name="id">Id of the issue to apply the command to.</param>
        /// <param name="command">The command to apply. A command might contain a string of attributes and their values - you can change multiple fields with one complex command.</param>
        /// <param name="comment">A comment to add to an issue.</param>
        /// <param name="disableNotifications">When true, no notifications about changes made with the specified command will be sent. Defaults to false.</param>
        /// <param name="runAs">Login name for a user on whose behalf the command should be applied.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="id"/> or <paramref name="command"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task ApplyCommand(string id, string command, string comment = null, bool disableNotifications = false, string runAs = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }
            
            var queryString = new List<string>(4);
            queryString.Add($"command={WebUtility.UrlEncode(command)}");
            
            if (!string.IsNullOrEmpty(comment))
            {
                queryString.Add($"comment={WebUtility.UrlEncode(comment)}");
            }
            if (disableNotifications)
            {
                queryString.Add($"disableNotifications=true");
            }
            if (!string.IsNullOrEmpty(runAs))
            {
                queryString.Add($"runAs={runAs}");
            }
            
            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/issue/{id}/execute?{query}", new MultipartFormDataContent());

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }
            
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Try reading the error message
                var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                if (responseJson["value"] != null)
                {
                    throw new YouTrackErrorException(responseJson["value"].Value<string>());
                }
                else
                {
                    throw new YouTrackErrorException(Strings.Exception_UnknownError);
                }
            }
            
            response.EnsureSuccessStatusCode();
        }
    }
}