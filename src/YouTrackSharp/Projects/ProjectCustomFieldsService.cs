using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouTrackSharp.Projects
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Project-Custom-Fields.html"> methods related to operations with custom fields of a project</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class ProjectCustomFieldsService : IProjectCustomFieldsService
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

        /// <inheritdoc />
        public async Task<ICollection<CustomField>> GetProjectCustomFields(string projectId)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/project/{projectId}/customfield");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<ICollection<CustomField>>(await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task<CustomField> GetProjectCustomField(string projectId, string customFieldName)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/project/{projectId}/customfield/{customFieldName}");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<CustomField>(await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
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

        /// <inheritdoc />
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