using System.Threading.Tasks;

namespace YouTrackSharp
{
    /// <summary>
    /// Extension methods for <see cref="Connection" />, providing easy access to all services.
    /// </summary>
    public static class ConnectionExtensions
    {
        /// <summary>
        /// Creates a <see cref="IYouTrackClient" />.
        /// </summary>
        /// <param name="connection">The <see cref="Connection" /> to create a service with.</param>
        /// <returns><see cref="IYouTrackClient" /> for working with YouTrack.</returns>
        public static async Task<IYouTrackClient> CreateYouTrackClientAsync(this Connection connection) 
            => new YouTrackClient(await connection.GetAuthenticatedHttpClient());
        
        /// <summary>
        /// Creates a <see cref="IYouTrackProjectsClient" />.
        /// </summary>
        /// <param name="connection">The <see cref="Connection" /> to create a service with.</param>
        /// <returns><see cref="IYouTrackProjectsClient" /> for working with YouTrack.</returns>
        public static async Task<IYouTrackProjectsClient> CreateYouTrackProjectsClientAsync(this Connection connection) 
            => new YouTrackProjectsClient(await connection.GetAuthenticatedHttpClient());
    }
}