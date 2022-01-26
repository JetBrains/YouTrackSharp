using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTrackSharp.Projects
{
    public interface IProjectCustomFieldsService
    {
        /// <summary>
        /// Get custom fields used in a project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-admin-projects-projectID-customFields.html#get_all-ProjectCustomField-method">Get Project Custom Fields</a>.</remarks>
        /// <param name="projectId">Id of the project to get the custom fields for.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="CustomField" /> that are accessible for currently logged in user.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<ICollection<CustomField>> GetProjectCustomFields(string projectId);

        /// <summary>
        /// Get a project's custom field by its name.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-admin-projects-projectID-customFields.html#get-ProjectCustomField-method">Get Project Custom Field</a>.</remarks>
        /// <param name="projectId">Id of the project to get the custom field for.</param>
        /// <param name="customFieldName">Name of the custom field to get.</param>
        /// <returns><see cref="CustomField" />.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<CustomField> GetProjectCustomField(string projectId, string customFieldName);

        /// <summary>
        /// Remove specified custom field from a project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-admin-projects-projectID-customFields.html#delete-ProjectCustomField-method">Delete a project custom field</a>.</remarks>
        /// <param name="projectId">Id of the project to delete the custom field for.</param>
        /// <param name="customFieldName">Name of the custom field to delete.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="customFieldName"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task DeleteProjectCustomField(string projectId, string customFieldName);

        /// <summary>
        /// Adds an existing custom field to a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-admin-projects-projectID-customFields.html#create-ProjectCustomField-method">Create a project custom field</a>.</remarks>
        /// <param name="projectId">Id of the project to add a custom field.</param>
        /// <param name="customField"><see cref="CustomField" /> to add to the project.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="customField"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task CreateProjectCustomField(string projectId, CustomField customField);

        /// <summary>
        /// Updates a custom field to a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-admin-projects-projectID-customFields.html#update-ProjectCustomField-method">Updates a project custom field</a>.</remarks>
        /// <param name="projectId">Id of the project.</param>
        /// <param name="customField"><see cref="CustomField" /> to update in project.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="customField"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task UpdateProjectCustomField(string projectId, CustomField customField);
    }
}