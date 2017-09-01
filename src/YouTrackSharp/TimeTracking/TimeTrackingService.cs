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
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/>, <paramref name="workItemId"/> or <paramref name="workItem"/> is null or empty.</exception>
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
        
        
        /// <summary>
        /// Deletes a work item for an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Delete-Existing-Work-Item.html">Delete Existing Work Item</a>.</remarks>
        /// <param name="issueId">Id of the issue to delete the work item for.</param>
        /// <param name="workItemId">Id of the work item to delete.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="workItemId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
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


		/// <summary>
		/// Get the current time tracking settings for a specific project.
		/// </summary>
		/// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-System-wide-Time-Tracking-Settings.html">Get System-wide Time Tracking Settings</a>.</remarks>
		/// <returns>System-wide <see cref="SystemWideTimeTrackingSettings" />.</returns>
		/// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
		public async Task<SystemWideTimeTrackingSettings> GetSystemWideTimeTrackingSettings()
	    {
		    var client = await _connection.GetAuthenticatedHttpClient();
		    var response = await client.GetAsync($"rest/admin/timetracking");

		    response.EnsureSuccessStatusCode();

			return JsonConvert.DeserializeObject<SystemWideTimeTrackingSettings>(await response.Content.ReadAsStringAsync());
	    }


		/// <summary>
		/// Updates the system wide time tracking settings.
		/// </summary>
		/// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/PUT-System-wide-Time-Tracking-Settings.html">Set system-wide time tracking settings: a list of working days in a week, and a number of hours in a working day</a>.</remarks>
		/// <param name="timeSettings"><see cref="SystemWideTimeTrackingSettings" />Parameter daysAWeek is ignored since Youtrack 5.1</param>
		/// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
		/// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
		public async Task UpdateSystemWideTimeTrackingSettings(SystemWideTimeTrackingSettings timeSettings)
	    {
		    if (timeSettings == null)
		    {
			    throw new ArgumentNullException(nameof(timeSettings));
		    }

		    var stringContent = new StringContent(JsonConvert.SerializeObject(timeSettings));
		    stringContent.Headers.ContentType = new MediaTypeHeaderValue(Constants.HttpContentTypes.ApplicationJson);

		    var client = await _connection.GetAuthenticatedHttpClient();
		    var response = await client.PutAsync($"rest/admin/timetracking", stringContent);

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
		/// Get the current time tracking settings for a specific project.
		/// </summary>
		/// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-Time-Tracking-Settings-for-a-Project.html">Get Time Tracking Settings for a Project</a>.</remarks>
		/// <param name="projectId">Id of the project to get timetracking settings for.</param>
		/// <returns><see cref="TimeTrackingSettings" />.</returns>
		/// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
		/// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
		public async Task<TimeTrackingSettings> GetTimeTrackingSettingsForProject(string projectId)
	    {
		    if (string.IsNullOrEmpty(projectId))
		    {
			    throw new ArgumentNullException(nameof(projectId));
		    }

		    var client = await _connection.GetAuthenticatedHttpClient();
		    var response = await client.GetAsync($"rest/admin/project/{projectId}/timetracking");

		    response.EnsureSuccessStatusCode();

			return JsonConvert.DeserializeObject<TimeTrackingSettings>(await response.Content.ReadAsStringAsync());
	    }


		/// <summary>
		/// Updates the current time tracking settings for a specific project.
		/// </summary>
		/// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/PUT-Time-Tracking-Settings-for-a-Project.html">Configure time tracking settings for a specific project</a>.</remarks>
		/// <param name="projectId">Id of the project to update.</param>
		/// <param name="timeTrackingSettings">Timetracking settings for this project.</param>
		/// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
		/// <exception cref="T:System.ArgumentNullException">When the <paramref name="timeTrackingSettings"/> is null.</exception>
		/// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
		/// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
		public async Task UpdateTimeTrackingSettingsForProject(string projectId, TimeTrackingSettings timeTrackingSettings)
	    {
		    if (string.IsNullOrEmpty(projectId))
		    {
			    throw new ArgumentNullException(nameof(projectId));
		    }

		    if (timeTrackingSettings == null)
		    {
			    throw new ArgumentNullException(nameof(timeTrackingSettings));
		    }

		    var stringContent = new StringContent(JsonConvert.SerializeObject(timeTrackingSettings));
		    stringContent.Headers.ContentType = new MediaTypeHeaderValue(Constants.HttpContentTypes.ApplicationJson);

		    var client = await _connection.GetAuthenticatedHttpClient();
		    var response = await client.PutAsync($"rest/admin/project/{projectId}/timetracking", stringContent);

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