using YouTrackSharp.AgileBoards;
using YouTrackSharp.Issues;
using YouTrackSharp.Management;
using YouTrackSharp.Projects;
using YouTrackSharp.TimeTracking;

namespace YouTrackSharp
{
    /// <summary>
    /// Extension methods for <see cref="Connection" />, providing easy access to all services.
    /// </summary>
    public static class ConnectionExtensions
    {
        /// <summary>
        /// Creates a <see cref="ProjectsService" />.
        /// </summary>
        /// <param name="connection">The <see cref="Connection" /> to create a service with.</param>
        /// <returns><see cref="ProjectsService" /> for working with YouTrack projects.</returns>
        public static ProjectsService CreateProjectsService(this Connection connection)
        {
            return new ProjectsService(connection);
        }

        /// <summary>
        /// Creates a <see cref="IssuesService" />.
        /// </summary>
        /// <param name="connection">The <see cref="Connection" /> to create a service with.</param>
        /// <returns><see cref="IssuesService" /> for working with YouTrack issues.</returns>
        public static IssuesService CreateIssuesService(this Connection connection)
        {
            return new IssuesService(connection);
        }

        /// <summary>
        /// Creates a <see cref="TimeTrackingService" />.
        /// </summary>
        /// <param name="connection">The <see cref="Connection" /> to create a service with.</param>
        /// <returns><see cref="TimeTrackingService" /> for working with YouTrack time tracking.</returns>
        public static TimeTrackingService CreateTimeTrackingService(this Connection connection)
        {
            return new TimeTrackingService(connection);
        }

        /// <summary>
        /// Creates a <see cref="UserManagementService" />.
        /// </summary>
        /// <param name="connection">The <see cref="Connection" /> to create a service with.</param>
        /// <returns><see cref="UserManagementService" /> for managing YouTrack users.</returns>
        public static UserManagementService CreateUserManagementService(this Connection connection)
        {
            return new UserManagementService(connection);
        }

        /// <summary>
        /// Creates a <see cref="TimeTrackingManagementService" />.
        /// </summary>
        /// <param name="connection">The <see cref="Connection" /> to create a service with.</param>
        /// <returns><see cref="TimeTrackingManagementService" /> for managing YouTrack time tracking settings.</returns>
        public static TimeTrackingManagementService CreateTimeTrackingManagementService(this Connection connection)
        {
            return new TimeTrackingManagementService(connection);
        }

        /// <summary>
        /// Creates a <see cref="ProjectCustomFieldsService" />.
        /// </summary>
        /// <param name="connection">The <see cref="Connection" /> to create a service with.</param>
        /// <returns><see cref="ProjectCustomFieldsService" /> for accessing custom project fields.</returns>
        public static ProjectCustomFieldsService ProjectCustomFieldsService(this Connection connection)
        {
            return new ProjectCustomFieldsService(connection);
        }

        /// <summary>
        /// Creates a <see cref="AgileBoardService" />.
        /// </summary>
        /// <param name="connection">The <see cref="Connection" /> to create a service with.</param>
        /// <returns><see cref="AgileBoardService" /> for working with YouTrack agile boards.</returns>
        public static AgileBoardService CreateAgileBoardsService(this Connection connection)
        {
            return new AgileBoardService(connection);
        }
    }
}