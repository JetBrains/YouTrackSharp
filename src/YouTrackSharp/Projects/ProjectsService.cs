using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Projects
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Projects-Related-Methods.html">YouTrack Projects Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class ProjectsService : IProjectsService
    {
        private readonly Connection _connection;

        /// <summary>
        /// Creates an instance of the <see cref="ProjectsService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public ProjectsService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc />
        public async Task<ICollection<Project>> GetAccessibleProjects(bool verbose = false)
        {
            var client = await _connection.GetAuthenticatedApiClient();
            ICollection<Generated.Project> apiProjects;

            if (!verbose)
            {
                apiProjects = await client.AdminProjectsGetAsync("name,shortName", 0, -1);
                return apiProjects.Select(Project.FromApiEntity).ToList();
            }

            var projects = new List<Project>();
            apiProjects = await client.AdminProjectsGetAsync("name,shortName,description", 0, -1);
            foreach (var apiProject in apiProjects)
            {
                var project = Project.FromApiEntity(apiProject);

                var apiFields =
                    await client.AdminProjectsCustomfieldsGetAsync(project.ShortName, "id,field(name),bundle(id)", 0, -1);
                var assigneesField = (UserProjectCustomField) apiFields.SingleOrDefault(f =>
                    f.Field.Name.Equals("Assignee", StringComparison.InvariantCultureIgnoreCase));
                var fixVersionsField = ( VersionProjectCustomField) apiFields.SingleOrDefault(f =>
                    f.Field.Name.Equals("Fix versions", StringComparison.InvariantCultureIgnoreCase));
                var affectedVersionsField = (VersionProjectCustomField) apiFields.SingleOrDefault(f =>
                    f.Field.Name.Equals("Affected versions", StringComparison.InvariantCultureIgnoreCase));
                var subsystemField = (OwnedProjectCustomField) apiFields.SingleOrDefault(f =>
                    f.Field.Name.Equals("Subsystem", StringComparison.InvariantCultureIgnoreCase));

                if (assigneesField != null)
                {
                    var apiUsers =
                        await client.AdminCustomfieldsettingsBundlesUserAggregatedusersAsync(assigneesField.Bundle.Id,
                            "id,login,fullName", 0, -1);
                    project.AssigneesLogin = apiUsers.Select(u => new SubValue<string>() {Value = u.Login}).ToList();
                    project.AssigneesFullName =
                        apiUsers.Select(u => new SubValue<string>() {Value = u.FullName}).ToList();
                }

                if (fixVersionsField != null)
                {
                    var apiValues =
                        await client.AdminCustomfieldsettingsBundlesVersionValuesGetAsync(fixVersionsField.Bundle.Id,
                            "id,name", 0, -1);
                    project.Versions = project.Versions.Concat(apiValues.Select(v => v.Name)).ToList();
                }

                if (affectedVersionsField != null)
                {
                    var apiValues =
                        await client.AdminCustomfieldsettingsBundlesVersionValuesGetAsync(affectedVersionsField.Bundle.Id,
                            "id,name", 0, -1);
                    project.Versions = project.Versions.Concat(apiValues.Select(v => v.Name)).ToList();
                }

                if (subsystemField != null)
                {
                    var apiValues =
                        await client.AdminCustomfieldsettingsBundlesOwnedfieldValuesGetAsync(subsystemField.Bundle.Id,
                            "id,name", 0, -1);
                    project.Subsystems = apiValues.Select(v => new SubValue<string>() {Value = v.Name}).ToList();
                }

                projects.Add(project);
            }

            return projects;
        }
    }
}