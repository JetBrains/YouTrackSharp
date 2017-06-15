using YouTrackSharp.Issues;
using YouTrackSharp.Projects;

namespace YouTrackSharp
{
    /// <summary>
    /// Extension methods for <see cref="Connection" /> , providing easy access to all services.
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
        public static IssuesService CreateIssueService(this Connection connection)
        {
            return new IssuesService(connection);
        }
    }
}