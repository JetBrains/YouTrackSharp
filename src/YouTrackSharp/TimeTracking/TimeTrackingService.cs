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
    public class TimeTrackingService : ITimeTrackingService
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
       
        /// <inheritdoc />
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
       
        /// <inheritdoc />
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
        
        /// <inheritdoc />
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
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(Constants.HttpContentTypes.ApplicationJson);

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
            const string marker = "timetracking/workitem/";
            var locationHeader = response.Headers.Location.ToString();
            
            return locationHeader.Substring(locationHeader.IndexOf(marker, StringComparison.OrdinalIgnoreCase) + marker.Length);
        }
        
        /// <inheritdoc />
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
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(Constants.HttpContentTypes.ApplicationJson);

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
        
        /// <inheritdoc />
        public async Task DeleteWorkItemForIssue(string issueId, string workItemId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.DeleteAsync($"rest/issue/{issueId}/timetracking/workitem/{workItemId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }
	}
}