﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YouTrackSharp.Management
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Users.html">administering user accounts in YouTrack</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class UserManagementService
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

        /// <summary>
        /// Get user by login name.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-User.html">Get user by login name</a>.</remarks>
        /// <returns>A <see cref="User" /> instance that represents the user matching <paramref name="username"/>.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="username"/> is null or empty.</exception>
        public async Task<User> GetUser(string username)
        {
            if (string.IsNullOrEmpty(username)) throw new ArgumentNullException(nameof(username));
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/user/{username}");
            
            response.EnsureSuccessStatusCode();
            
            return JsonConvert.DeserializeObject<User>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Get a list of all available registered users.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-Users.html">Get a list of all available registered users</a>.</remarks>
        /// <param name="filter">Search query (part of user login, name or email).</param>
        /// <param name="group">Filter by group (groupID).</param>
        /// <param name="role">Filter by role.</param>
        /// <param name="project">Filter by project (projectID)</param>
        /// <param name="permission">Filter by permission.</param>
        /// <param name="onlineOnly">When <value>true</value>, get only users which are currently online. Defaults to <value>false</value>.</param>
        /// <param name="start">Paginator mode (takes 10 records).</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Users" /> instances.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<User>> GetUsers(string filter = null, string group = null, string role = null,
            string project = null, string permission = null, bool onlineOnly = false, int start = 0)
        {
            var queryString = new List<string>(7);
            if (!string.IsNullOrEmpty(filter))
            {
                queryString.Add($"q={WebUtility.UrlEncode(filter)}");
            }
            if (!string.IsNullOrEmpty(group))
            {
                queryString.Add($"group={WebUtility.UrlEncode(group)}");
            }
            if (!string.IsNullOrEmpty(role))
            {
                queryString.Add($"role={WebUtility.UrlEncode(role)}");
            }
            if (!string.IsNullOrEmpty(project))
            {
                queryString.Add($"project={WebUtility.UrlEncode(project)}");
            }
            if (!string.IsNullOrEmpty(permission))
            {
                queryString.Add($"permission={WebUtility.UrlEncode(permission)}");
            }
            if (onlineOnly)
            {
                queryString.Add($"onlineOnly=true");
            }
            if (start > 0)
            {
                queryString.Add($"start={start}");
            }

            var query = string.Join("&", queryString);
            
            return await GetUsersFromPath($"rest/admin/user?{query}");
        }

        private async Task<List<User>> GetUsersFromPath(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync(path);
            
            response.EnsureSuccessStatusCode();

            var users = new List<User>();
            var userRefs = JArray.Parse(await response.Content.ReadAsStringAsync());
            foreach (var userRef in userRefs)
            {
                users.Add(await GetUser(userRef["login"].Value<string>()));
            }

            return users;
        }
    }
}