using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YouTrackSharp.Management
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Time-Tracking-Settings-Methods.html">administering Time Tracking Settings in YouTrack</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class TimeTrackingManagementService
    {
        private readonly Connection _connection;

        /// <summary>
        /// Creates an instance of the <see cref="TimeTrackingManagementService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public TimeTrackingManagementService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
        
		/// <summary>
		/// Get the current system-wide time tracking settings.
		/// </summary>
		/// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-System-wide-Time-Tracking-Settings.html">Get System-wide Time Tracking Settings</a>.</remarks>
		/// <returns>System-wide <see cref="SystemWideTimeTrackingSettings" />.</returns>
		/// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
		public async Task<SystemWideTimeTrackingSettings> GetSystemWideTimeTrackingSettings()
	    {
		    var client = await _connection.GetAuthenticatedHttpClient();
		    var response = await client.GetAsync("rest/admin/timetracking");

		    response.EnsureSuccessStatusCode();
            
            return JsonConvert.DeserializeObject<SystemWideTimeTrackingSettings>(await response.Content.ReadAsStringAsync());
	    }


	    /// <summary>
	    /// Updates the system-wide time tracking settings.
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
		    var response = await client.PutAsync("rest/admin/timetracking", stringContent);

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