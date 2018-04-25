using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Issues-Related-Methods.html">YouTrack Issues Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public partial class IssuesService
    {
        private readonly Connection _connection;
        
        private static readonly string[] ReservedFields = 
        {
            "id", "entityid", "jiraid", "summary", "description", "markdown"
        };

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
        /// <param name="issueId">Id of an issue to get.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then issue description in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>The <see cref="Issue" /> that matches the requested <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Issue> GetIssue(string issueId, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}?wikifyDescription={wikifyDescription}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            
            return JsonConvert.DeserializeObject<Issue>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Checks whether an issue exists on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Check-that-an-Issue-Exists.html">Check that an Issue Exists</a>.</remarks>
        /// <param name="issueId">Id of an issue to check.</param>
        /// <returns><value>True</value> if the issue exists, otherwise <value>false</value>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<bool> Exists(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}/exists");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();

            return true;
        }

        /// <summary>
        /// Creates an issue on the server in a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Create-New-Issue.html">Create New Issue</a>.</remarks>
        /// <param name="projectId">Id of the project to create an issue in.</param>
        /// <param name="issue">The <see cref="Issue" /> to create. At the minimum needs the Summary field populated.</param>
        /// <returns>The newly created <see cref="Issue" />'s id on the server.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<string> CreateIssue(string projectId, Issue issue)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            
            var queryString = new List<string>(4);
            queryString.Add($"project={projectId}");
            
            if (!string.IsNullOrEmpty(issue.Summary))
            {
                queryString.Add($"summary={WebUtility.UrlEncode(issue.Summary)}");
            }
            if (!string.IsNullOrEmpty(issue.Description))
            {
                queryString.Add($"description={WebUtility.UrlEncode(issue.Description)}");
            }
            if (issue.IsMarkdown)
            {
                queryString.Add($"markdown=true");
            }
            
            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PutAsync($"rest/issue?{query}", new MultipartFormDataContent());

            response.EnsureSuccessStatusCode();
            
            // Extract issue id from Location header response
            const string marker = "rest/issue/";
            var locationHeader = response.Headers.Location.ToString();
            
            var issueId = locationHeader.Substring(locationHeader.IndexOf(marker, StringComparison.OrdinalIgnoreCase) + marker.Length);

            // For every custom field, apply a command
            var customFields = issue.Fields
                .Where(field => !ReservedFields.Contains(field.Name.ToLower()))
                .ToDictionary(field => field.Name, field => field.Value);
            
            foreach (var customField in customFields)
            {
                if (!(customField.Value is string) && customField.Value is System.Collections.IEnumerable enumerable)
                {
                    await ApplyCommand(issueId, $"{customField.Key} {string.Join(" ", enumerable.OfType<string>())}", string.Empty);
                }
                else if (customField.Value is DateTime dateTime)
                {
                    await ApplyCommand(issueId, $"{customField.Key} {dateTime:s}", string.Empty);
                }
                else if (customField.Value is DateTimeOffset dateTimeOffset)
                {
                    await ApplyCommand(issueId, $"{customField.Key} {dateTimeOffset:s}", string.Empty);
                }
                else
                {
                    await ApplyCommand(issueId, $"{customField.Key} {customField.Value}", string.Empty);
                }
            }
            
            // Add comments?
            foreach (var issueComment in issue.Comments)
            {
                await ApplyCommand(issueId, "comment", issueComment.Text, runAs: issueComment.Author);
            }
            
            // Add tags?
            foreach (var issueTag in issue.Tags)
            {
                await ApplyCommand(issueId, $"tag {issueTag.Value}");
            }
            
            return issueId;
        }

        /// <summary>
        /// Updates an issue on the server in a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Update-an-Issue.html">Update an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to update.</param>
        /// <param name="summary">Updated summary of the issue.</param>
        /// <param name="description">Updated description of the issue.</param>
        /// <param name="isMarkdown">Is the updated description of the issue in Markdown format? Setting the format to Markdown is supported in YouTrack versions 2018.2 and later.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task UpdateIssue(string issueId, string summary = null, string description = null, bool? isMarkdown = null)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            if (summary == null && description == null)
            {
                return;
            }
            
            var queryString = new List<string>(3);
            if (!string.IsNullOrEmpty(summary))
            {
                queryString.Add($"summary={WebUtility.UrlEncode(summary)}");
            }
            if (!string.IsNullOrEmpty(description))
            {
                queryString.Add($"description={WebUtility.UrlEncode(description)}");
            }
            if (isMarkdown.HasValue)
            {
                queryString.Add($"markdown={isMarkdown.ToString().ToLowerInvariant()}");
            }
            
            var query = string.Join("&", queryString);

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/issue/{issueId}?{query}", new MultipartFormDataContent());

            response.EnsureSuccessStatusCode();
        }
        
        /// <summary>
        /// Applies a command to a specific issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Apply-Command-to-an-Issue.html">Apply Command to an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to apply the command to.</param>
        /// <param name="command">The command to apply. A command might contain a string of attributes and their values - you can change multiple fields with one complex command.</param>
        /// <param name="comment">A comment to add to an issue.</param>
        /// <param name="disableNotifications">When <value>true</value>, no notifications about changes made with the specified command will be sent. Defaults to <value>false</value>.</param>
        /// <param name="runAs">Login name for a user on whose behalf the command should be applied.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="command"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task ApplyCommand(string issueId, string command, string comment = null, bool disableNotifications = false, string runAs = null)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
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
                queryString.Add("disableNotifications=true");
            }
            if (!string.IsNullOrEmpty(runAs))
            {
                queryString.Add($"runAs={runAs}");
            }
            
            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/issue/{issueId}/execute?{query}", new MultipartFormDataContent());
            
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
        
        /// <summary>
        /// Deletes an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Delete-an-Issue.html">Delete an Issue</a>.</remarks>
        /// <param name="issueId">Id of an issue to delete.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task DeleteIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.DeleteAsync($"rest/issue/{issueId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }
        /// <summary>
        /// Get links for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Links-of-an-Issue.html">Get Links of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get links for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Link" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<IEnumerable<Link>> GetLinksForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}/link");

            response.EnsureSuccessStatusCode();
            
            return JsonConvert.DeserializeObject<IEnumerable<Link>>(await response.Content.ReadAsStringAsync());
        }
    }
}