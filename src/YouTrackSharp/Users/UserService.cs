using System;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouTrackSharp.Users
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/User-Related-Methods.html">YouTrack User Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class UserService : IUserService
    {
        private readonly Connection _connection;

        /// <summary>
        /// Creates an instance of the <see cref="UserService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public UserService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc />
        public async Task<User> GetCurrentUserInfo()
        {
            var client = await _connection.GetAuthenticatedAPIClient();
            var me = await client.MeAsync();

            return new User(me);
        }
    }
}