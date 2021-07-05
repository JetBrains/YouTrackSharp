using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YouTrackSharp.Management
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Users.html">administering user accounts in YouTrack</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class UserManagementService : IUserManagementService
    {
        private readonly Connection _connection;

        /// <summary>
        /// Creates an instance of the <see cref="UserManagementService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public UserManagementService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc />
        public async Task<User> GetUser(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            
            var client = await _connection.GetAuthenticatedApiClient();
            var foundUsers = await GetUsers("login: {" + username + "}");
            
            return foundUsers.FirstOrDefault();
        }

        /// <inheritdoc />
        public async Task<ICollection<User>> GetUsers(string filter = null, string group = null, string role = null,
            string project = null, string permission = null, int start = 0, int take = 10)
        {
            var query = string.Empty;
            if (!string.IsNullOrEmpty(filter))
            {
                query += filter + " ";
            }
            if (!string.IsNullOrEmpty(group) && !group.Equals("All Users", StringComparison.InvariantCultureIgnoreCase))
            {
                query += "group:" + group + " ";
            }
            if (!string.IsNullOrEmpty(role))
            {
                query += "access(with:{" + role + "}) ";
            }
            if (!string.IsNullOrEmpty(project))
            {
                query += "access(project:" + project + ") ";
            }
            if (!string.IsNullOrEmpty(permission))
            {
                query += "access(with:{" + permission + "}) ";
            }

            var client = await _connection.GetAuthenticatedApiClient();
            var response = await client.HubApiUsersGetAsync(query,
                "id,login,name,profile(email(email),jabber(jabber))", 
                start, take);
            
            return response.Users.Select(User.FromApiEntity).ToList();
        }

        /// <inheritdoc />
        public async Task CreateUser(string username, string fullName, string email, string jabber, string password)
        {
            var queryString = new Dictionary<string, string>(4);
            if (!string.IsNullOrEmpty(fullName))
            {
                queryString.Add("fullName", fullName);
            }
            if (!string.IsNullOrEmpty(email))
            {
                queryString.Add("email", email);
            }
            if (!string.IsNullOrEmpty(jabber))
            {
                queryString.Add("jabber", jabber);
            }
            if (!string.IsNullOrEmpty(password))
            {
                queryString.Add("password", Uri.EscapeDataString(password));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PutAsync($"rest/admin/user/{username}", new FormUrlEncodedContent(queryString));
            
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task UpdateUser(string username, string fullName = null, string email = null, string jabber = null, string password = null)
        {
            var queryString = new Dictionary<string, string>(4);
            if (!string.IsNullOrEmpty(fullName))
            {
                queryString.Add("fullName", fullName);
            }
            if (!string.IsNullOrEmpty(email))
            {
                queryString.Add("email", email);
            }
            if (!string.IsNullOrEmpty(jabber))
            {
                queryString.Add("jabber", jabber);
            }
            if (!string.IsNullOrEmpty(password))
            {
                queryString.Add("password", Uri.EscapeDataString(password));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/admin/user/{username}", new FormUrlEncodedContent(queryString));
            
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task DeleteUser(string username)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.DeleteAsync($"rest/admin/user/{username}");
            
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task MergeUsers(string usernameToMerge, string targetUser)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/admin/user/{targetUser}/merge/{usernameToMerge}", new StringContent(string.Empty));
            
            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task<ICollection<Group>> GetGroupsForUser(string username)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/user/{username}/group");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<List<Group>>(
                await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task AddUserToGroup(string username, string group)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/admin/user/{username}/group/{Uri.EscapeDataString(group)}", new StringContent(string.Empty));

            response.EnsureSuccessStatusCode();
        }

        /// <inheritdoc />
        public async Task RemoveUserFromGroup(string username, string group)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.DeleteAsync($"rest/admin/user/{username}/group/{Uri.EscapeDataString(group)}");

            response.EnsureSuccessStatusCode();
        }
    }
}