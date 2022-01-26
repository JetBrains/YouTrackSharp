using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouTrackSharp.Management
{
	/// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-admin-timeTrackingSettings.html">administering Time Tracking Settings in YouTrack</a>.
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
		    var client = await _connection.GetAuthenticatedApiClient();
		    var response =
			    await client.AdminTimetrackingsettingsWorktimesettingsGetAsync("workDays,minutesADay,daysAWeek");
			
		    return SystemWideTimeTrackingSettings.FromApiEntity(response);
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

		    var client = await _connection.GetAuthenticatedApiClient();
		    var response =
			    await client.AdminTimetrackingsettingsWorktimesettingsPostAsync(null, timeSettings.ToApiEntity());
	    }

		/// <inheritdoc />
		public async Task<TimeTrackingSettings> GetTimeTrackingSettingsForProject(string projectId)
	    {
		    if (string.IsNullOrEmpty(projectId))
		    {
			    throw new ArgumentNullException(nameof(projectId));
		    }

		    var client = await _connection.GetAuthenticatedApiClient();
		    var response = await client.AdminProjectsTimetrackingsettingsGetAsync(projectId,
			    "enabled,estimate(field(id,name)),timeSpent(field(id,name))");

		    return TimeTrackingSettings.FromApiEntity(response);
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

		    var client = await _connection.GetAuthenticatedApiClient();
		    var pcfList = await client.AdminCustomfieldsettingsCustomfieldsGetAsync("id,name", 0, -1);

		    await client.AdminProjectsTimetrackingsettingsPostAsync(projectId, "id",
			    timeTrackingSettings.ToApiEntity(pcfList));
		    
	    }
    }
}