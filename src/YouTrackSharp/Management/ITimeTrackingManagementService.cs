using System.Threading.Tasks;

namespace YouTrackSharp.Management
{
    public interface ITimeTrackingManagementService
    {
        /// <summary>
        /// Get the current system-wide time tracking settings.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-System-wide-Time-Tracking-Settings.html">Get System-wide Time Tracking Settings</a>.</remarks>
        /// <returns>System-wide <see cref="SystemWideTimeTrackingSettings" />.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<SystemWideTimeTrackingSettings> GetSystemWideTimeTrackingSettings();

        /// <summary>
        /// Updates the system-wide time tracking settings.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/PUT-System-wide-Time-Tracking-Settings.html">Set system-wide time tracking settings: a list of working days in a week, and a number of hours in a working day</a>.</remarks>
        /// <param name="timeSettings"><see cref="SystemWideTimeTrackingSettings" />Parameter daysAWeek is ignored since Youtrack 5.1</param>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task UpdateSystemWideTimeTrackingSettings(SystemWideTimeTrackingSettings timeSettings);

        /// <summary>
        /// Get the current time tracking settings for a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-Time-Tracking-Settings-for-a-Project.html">Get Time Tracking Settings for a Project</a>.</remarks>
        /// <param name="projectId">Id of the project to get timetracking settings for.</param>
        /// <returns><see cref="TimeTrackingSettings" />.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<TimeTrackingSettings> GetTimeTrackingSettingsForProject(string projectId);

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
        Task UpdateTimeTrackingSettingsForProject(string projectId, TimeTrackingSettings timeTrackingSettings);
    }
}