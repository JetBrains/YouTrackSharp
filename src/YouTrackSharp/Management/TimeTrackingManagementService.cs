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
    public class TimeTrackingManagementService : ITimeTrackingManagementService
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
        
		/// <inheritdoc />
		public async Task<SystemWideTimeTrackingSettings> GetSystemWideTimeTrackingSettings()
	    {
		    var client = await _connection.GetAuthenticatedHttpClient();
		    var response = await client.GetAsync("rest/admin/timetracking");

		    response.EnsureSuccessStatusCode();
            
            return JsonConvert.DeserializeObject<SystemWideTimeTrackingSettings>(await response.Content.ReadAsStringAsync());
	    }

		/// <inheritdoc />
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

		/// <inheritdoc />
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

		/// <inheritdoc />
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