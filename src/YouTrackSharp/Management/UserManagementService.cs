using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using YouTrackSharp.Generated;

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
                "id,banned,banBadge,banReason,login,name,profile(email(email),jabber(jabber))",
                start, take);
            
            return response.Users.Select(User.FromApiEntity).ToList();
            
            //TODO isOnline? get from youtrack directly and filter
        }

        /// <inheritdoc />
        public async Task CreateUser(string username, string fullName, string email, string jabber, string password)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }

            var user = new HubApiUser() {Login = username};
            var details = new EmailuserdetailsJSON();
            
            if (!string.IsNullOrEmpty(fullName))
            {
                user.Name = fullName;
            }
            if (!string.IsNullOrEmpty(email))
            {
                details.Email = new HubApiEmail() {Email = email};
            }
            if (!string.IsNullOrEmpty(jabber))
            {
                details.Jabber = new HubApiJabber() {Jabber = jabber};
            }
            if (!string.IsNullOrEmpty(password))
            {
                details.Password = new PlainpasswordJSON() {Value = password};
            }
            
            user.Details = new List<DetailsJSON>() {details};
            
            var client = await _connection.GetAuthenticatedApiClient();
            
            await client.HubUsersPostAsync("id", user);
        }

        /// <inheritdoc />
        public async Task UpdateUser(string username, string fullName = null, string email = null, string jabber = null, string password = null)
        {
            if (string.IsNullOrEmpty(username))
            {
                throw new ArgumentNullException(nameof(username));
            }
            
            var client = await _connection.GetAuthenticatedApiClient();
            var users = await client.HubApiUsersGetAsync("login: {" + username + "}", "id,details(id,login)");
            var user = users.Users.FirstOrDefault();

            if (user == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                    "Could not find user with login " + username,
                    null, null);
            }

            if (!string.IsNullOrEmpty(fullName))
            {
                user.Name = fullName;
            }
            if (!string.IsNullOrEmpty(email))
            {
                user.Profile.Email = new HubApiEmail() {Email = email};
            }
            if (!string.IsNullOrEmpty(jabber))
            {
                user.Profile.Jabber = new HubApiJabber() {Jabber = jabber};
            }
            
            await client.HubUsersPostAsync(user.Id, "id", user);

            if (!string.IsNullOrEmpty(password))
            {
                var detail = user.Details.SingleOrDefault(d => d is EmailuserdetailsJSON);
                if (detail == null)
                {
                    throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                        "Could not change password for user " + username + ": no single EmailUserDetails found",
                        null, null);
                }
                
                await client.HubUserdetailsPostAsync(detail.Id, "id",
                    new EmailuserdetailsJSON() {Password = new PlainpasswordJSON() {Value = password}});
            }
        }

        /// <inheritdoc />
        public async Task DeleteUser(string username, string successor)
        {
            var userToDelete = await GetUser(username);
            var userToInherit = await GetUser(successor);
            
            if (userToDelete == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                    "Could not find user with login " + username,
                    null, null);
            }
            
            if (userToInherit == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                    "Could not find target user with login " + successor,
                    null, null);
            }
            
            var client = await _connection.GetAuthenticatedApiClient();

            await client.HubUsersDeleteAsync(userToDelete.RingId, userToInherit.RingId);
        }

        /// <inheritdoc />
        public async Task MergeUsers(string usernameToMerge, string targetUser, bool promoteBan = true)
        {
            var source = await GetUser(usernameToMerge);
            var target = await GetUser(targetUser);

            if (source == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                    "Could not find user with login " + usernameToMerge,
                    null, null);
            }
            
            if (target == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                    "Could not find target user with login " + targetUser,
                    null, null);
            }

            var users = new List<HubApiUser>()
            {
                new HubApiUser() {Id = source.RingId},
                new HubApiUser() {Id = target.RingId}
            };
            var client = await _connection.GetAuthenticatedApiClient();
            
            if (target.Banned || !(promoteBan && source.Banned))
            {
                await client.HubUsersMergePostAsync("id", targetUser, target.FullName, target.Email, target.Banned,
                    target.BanBadge, target.BanReason, target.RingId, users);
            }
            else
            {
                await client.HubUsersMergePostAsync("id", targetUser, target.FullName, target.Email, source.Banned,
                    source.BanBadge, source.BanReason, target.RingId, users);
            }
        }

        /// <inheritdoc />
        public async Task<ICollection<Group>> GetGroupsForUser(string username)
        {
            var client = await _connection.GetAuthenticatedApiClient();
            
            var user = await GetUser(username);
            
            if (user == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                    "Could not find user with login " + user, null, null);
            }
            
            var response = await client.HubApiUsergroupsGetAsync("user:" + user.RingId + " or is:allUsers", "id,name", 0, -1);
            
            return response.Usergroups.Select(Group.FromApiEntity).ToList();
        }

        /// <inheritdoc />
        public async Task AddUserToGroup(string username, string group)
        {
            var client = await _connection.GetAuthenticatedApiClient();
            
            var user = await GetUser(username);
            var apiGroups = await client.HubApiUsergroupsGetAsync("name:{" + group + "}", "id", 0, -1);
            var apiGroup = apiGroups.Usergroups.FirstOrDefault();
            
            if (user == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                    "Could not find user with login " + username, null, null);
            }
            if (apiGroup == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                    "Could not find group with name " + group, null, null);
            }

            await client.HubUsergroupsUsersPostAsync(apiGroup.Id, "id", new HubApiUser(){Id = user.RingId});
        }

        /// <inheritdoc />
        public async Task RemoveUserFromGroup(string username, string group)
        {
            var client = await _connection.GetAuthenticatedApiClient();
            
            var user = await GetUser(username);
            var apiGroups = await client.HubApiUsergroupsGetAsync("name:{" + group + "}", "id", 0, -1);
            var apiGroup = apiGroups.Usergroups.FirstOrDefault();
            
            if (user == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                    "Could not find user with login " + username, null, null);
            }
            if (apiGroup == null)
            {
                throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                    "Could not find group with name " + group, null, null);
            }

            await client.HubUsergroupsUsersDeleteAsync(apiGroup.Id, user.RingId);
        }
    }
}