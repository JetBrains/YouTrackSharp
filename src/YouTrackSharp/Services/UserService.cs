using System;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YouTrackSharp.Entities;

namespace YouTrackSharp.Services
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/User-Related-Methods.html">YouTrack User Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class UserService
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

        /// <summary>
        /// Get info about currently logged in user.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Info-For-Current-User.html">Get Info For Current User</a>.</remarks>
        /// <returns>A <see cref="UserInfo" /> instance that represents the currently logged in user.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<UserInfo> GetCurrentUserInfo()
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync("rest/user/current");
            
            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<UserInfo>(await response.Content.ReadAsStringAsync());
        }
    }
}