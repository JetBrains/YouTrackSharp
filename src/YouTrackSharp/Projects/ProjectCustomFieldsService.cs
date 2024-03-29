﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Projects
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-admin-projects-projectID-customFields.html"> methods related to operations with custom fields of a project</a>.
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
            var client = await _connection.GetAuthenticatedApiClient();
            var response = await client.AdminProjectsCustomfieldsGetAsync(projectId,
                "id,field(name,fieldType(id,name)),canBeEmpty,emptyFieldText", 0, -1);
            
            return response.Select(CustomField.FromApiEntity).ToList();
        }

        /// <inheritdoc />
        public async Task<CustomField> GetProjectCustomField(string projectId, string customFieldName)
        {
            var fields = await GetProjectCustomFields(projectId);
            var field =  fields.SingleOrDefault(f => f.Name == customFieldName);

            if (field == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.NotFound,
                    "Project custom field [ " + customFieldName + " ] not found ", null, null);
            }

            return field;
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

            var client = await _connection.GetAuthenticatedApiClient();

            CustomField field;
            try
            {
                field = await GetProjectCustomField(projectId, customFieldName);
            }
            catch (YouTrackErrorException e)
            {
                if (e.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    return;
                }

                throw;
            }
            
            await client.AdminProjectsCustomfieldsDeleteAsync(projectId, field.Id);
        }

        /// <inheritdoc />
        public async Task CreateProjectCustomField(string projectId, CustomField customField)
        {
            if (string.IsNullOrEmpty(customField?.Name))
            {
                throw new ArgumentNullException(nameof(customField));
            }

            var client = await _connection.GetAuthenticatedApiClient();
            var allFields = await client.AdminCustomfieldsettingsCustomfieldsGetAsync("id,name,fieldType(id,name)", 0, -1);
            var bundleField = customField.ToApiEntity(allFields);
            
            var projectField = await client.AdminProjectsCustomfieldsPostAsync(projectId, "id", bundleField);
            await client.AdminProjectsCustomfieldsPostAsync(projectId, projectField.Id, "id", bundleField);
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

            var client = await _connection.GetAuthenticatedApiClient();
            var projectField = await GetProjectCustomField(projectId, customField.Name);
            var allFields = await client.AdminCustomfieldsettingsCustomfieldsGetAsync("id,name,fieldType(id,name)", 0, -1);
            await client.AdminProjectsCustomfieldsPostAsync(projectId, projectField.Id, customField.ToApiEntity(allFields));
        }
    }
}