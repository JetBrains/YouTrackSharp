using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YouTrackSharp.TimeTracking
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Time-Tracking-User-Methods.html">YouTrack Time Tracking User Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public partial class TimeTrackingService
    {
        private readonly Connection _connection;
        
        /// <summary>
        /// Creates an instance of the <see cref="TimeTrackingService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public TimeTrackingService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
       
        /// <summary>
        /// Get work types for a specific project from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-Work-Types-for-a-Project.html">GET Work Types for a Project</a>.</remarks>
        /// <param name="projectId">Id of the issue to get work items for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="WorkType" /> for the requested project <paramref name="projectId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<IEnumerable<WorkType>> GetWorkTypesForProject(string projectId)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/project/{projectId}/timetracking/worktype");

            response.EnsureSuccessStatusCode();
            
            return JsonConvert.DeserializeObject<IEnumerable<WorkType>>(await response.Content.ReadAsStringAsync());
        }
       
        /// <summary>
        /// Get work items for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Available-Work-Items-of-Issue.html">Get Available Work Items of Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get work items for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="WorkItem" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<IEnumerable<WorkItem>> GetWorkItemsForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}/timetracking/workitem");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<IEnumerable<WorkItem>>(await response.Content.ReadAsStringAsync());
        }
        
        /// <summary>
        /// Creates a work item for an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Create-New-Work-Item.html">Create New Work Item</a>.</remarks>
        /// <param name="issueId">Id of the issue to create the work item for.</param>
        /// <param name="workItem">The <see cref="WorkItem"/> to create.</param>
        /// <returns>The newly created <see cref="WorkItem" />'s id on the server.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="workItem"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<string> CreateWorkItemForIssue(string issueId, WorkItem workItem)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            var stringContent = new StringContent(JsonConvert.SerializeObject(workItem));
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/issue/{issueId}/timetracking/workitem", stringContent);

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
            
            // Extract work item id from Location header response
            var marker = "timetracking/workitem/";
            var locationHeader = response.Headers.Location.ToString();
            
            return locationHeader.Substring(locationHeader.IndexOf(marker, StringComparison.OrdinalIgnoreCase) + marker.Length);
        }
        
        /// <summary>
        /// Updates a work item for an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Edit-Existing-Work-Item.html">Edit Existing Work Item</a>.</remarks>
        /// <param name="issueId">Id of the issue to update the work item for.</param>
        /// <param name="workItemId">Id of the work item to update.</param>
        /// <param name="workItem">The <see cref="WorkItem"/> to update.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="workItem"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task UpdateWorkItemForIssue(string issueId, string workItemId, WorkItem workItem)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            if (string.IsNullOrEmpty(workItemId))
            {
                throw new ArgumentNullException(nameof(workItemId));
            }
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            var stringContent = new StringContent(JsonConvert.SerializeObject(workItem));
            stringContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PutAsync($"rest/issue/{issueId}/timetracking/workitem/{workItemId}", stringContent);

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