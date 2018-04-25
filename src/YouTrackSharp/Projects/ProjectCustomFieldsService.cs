using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YouTrackSharp.Management;

namespace YouTrackSharp.Projects
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Project-Custom-Fields.html"> methods related to operations with custom fields of a project</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class ProjectCustomFieldsService
    {
        private readonly Connection _connection;

        /// <summary>
        /// Creates an instance of the <see cref="ProjectCustomFieldsService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public ProjectCustomFieldsService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Get custom fields used in a project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-Project-Custom-Fields.html">Get Project Custom Fields</a>.</remarks>
        /// <param name="projectId">Id of the project to get the custom fields for.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="CustomField" /> that are accessible for currently logged in user.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<CustomField>> GetProjectCustomFields(string projectId)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/project/{projectId}/customfield");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<ICollection<CustomField>>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Get a project's custom field by its name.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-Project-Custom-Field.html">Get Project Custom Field</a>.</remarks>
        /// <param name="projectId">Id of the project to get the custom field for.</param>
        /// <param name="customFieldName">Name of the custom field to get.</param>
        /// <returns><see cref="CustomField" />.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<CustomField> GetProjectCustomField(string projectId, string customFieldName)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/project/{projectId}/customfield/{customFieldName}");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<CustomField>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Remove specified custom field from a project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/DELETE-Project-Custom-Field.html">Delete a project custom field</a>.</remarks>
        /// <param name="projectId">Id of the project to delete the custom field for.</param>
        /// <param name="customFieldName">Name of the custom field to delete.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="customFieldName"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task DeleteProjectCustomField(string projectId, string customFieldName)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            if (string.IsNullOrEmpty(customFieldName))
            {
                throw new ArgumentNullException(nameof(customFieldName));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.DeleteAsync($"rest/admin/project/{projectId}/customfield/{customFieldName}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Adds an existing custom field to a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/PUT-Project-Custom-Field.html">Create a project custom field</a>.</remarks>
        /// <param name="projectId">Id of the project to add a custom field.</param>
        /// <param name="customField"><see cref="CustomField" /> to add to the project.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="customField"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task CreateProjectCustomField(string projectId, CustomField customField)
        {
            if (string.IsNullOrEmpty(customField?.Name))
            {
                throw new ArgumentNullException(nameof(customField));
            }

            var query = string.Empty;
            if (!string.IsNullOrEmpty(customField.EmptyText))
            {
                query = $"?emptyFieldText={WebUtility.UrlEncode(customField.EmptyText)}";
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PutAsync($"rest/admin/project/{projectId}/customfield/{customField.Name}{query}", new MultipartContent());

            response.EnsureSuccessStatusCode();
        }

        /// <summary>
        /// Updates a custom field to a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/POST-Project-Custom-Field.html">Updates a project custom field</a>.</remarks>
        /// <param name="projectId">Id of the project.</param>
        /// <param name="customField"><see cref="CustomField" /> to update in project.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="customField"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task UpdateProjectCustomField(string projectId, CustomField customField)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            if (string.IsNullOrEmpty(customField?.Name))
            {
                throw new ArgumentNullException(nameof(customField));
            }

            var query = string.Empty;
            if (!string.IsNullOrEmpty(customField.EmptyText))
            {
                query = $"?emptyFieldText={WebUtility.UrlEncode(customField.EmptyText)}";
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/admin/project/{projectId}/customfield/{customField.Name}{query}", new MultipartContent());

            response.EnsureSuccessStatusCode();
        }
    }
}