using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTrackSharp.Management
{
    public interface IUserManagementService
    {
        /// <summary>
        /// Get user by login name.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-User.html">Get user by login name</a>.</remarks>
        /// <returns>A <see cref="User" /> instance that represents the user matching <paramref name="username"/> or <value>null</value> if the user does not exist.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="username"/> is null or empty.</exception>
        Task<User> GetUser(string username);

        /// <summary>
        /// Get a list of available registered users, paged per 10. Use the <paramref name="start"/> parameter to get subsequent results.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-Users.html">Get a list of all available registered users</a>.</remarks>
        /// <param name="filter">Search query (part of user login, name or email).</param>
        /// <param name="group">Filter by group (groupID).</param>
        /// <param name="role">Filter by role.</param>
        /// <param name="project">Filter by project (projectID)</param>
        /// <param name="permission">Filter by permission.</param>
        /// <param name="start">Paginator mode.</param>
        /// <param name="take">Paginator page size (default is 10 records).</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="User" /> instances.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<ICollection<User>> GetUsers(string filter = null, string group = null, string role = null,
            string project = null, string permission = null, int start = 0, int take = 10);

        /// <summary>
        /// Create a new user.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/PUT-User.html">Create a new user</a>.</remarks>
        /// <param name="username">Login name of the user to be created.</param>
        /// <param name="fullName">Full name of a new user.</param>
        /// <param name="email">E-mail address of the user.</param>
        /// <param name="jabber">Jabber address for the new user.</param>
        /// <param name="password">Password for the new user.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task CreateUser(string username, string fullName, string email, string jabber, string password);

        /// <summary>
        /// Updates a user.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/POST-User.html">Update a user</a>.</remarks>
        /// <param name="username">Login name of the user to be updated.</param>
        /// <param name="fullName">Full name of a user.</param>
        /// <param name="email">E-mail address of the user.</param>
        /// <param name="jabber">Jabber address for the user.</param>
        /// <param name="password">Password for the user.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task UpdateUser(string username, string fullName = null, string email = null, string jabber = null, string password = null);

        /// <summary>
        /// Delete specific user account.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/DELETE-User.html">Delete a user</a>.</remarks>
        /// <param name="username">Login name of the user to be deleted.</param>
        /// <param name="successor">Login name of the user to inherit all data from user being deleted.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task DeleteUser(string username, string successor);

        /// <summary>
        /// Merge users.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Merge-Users.html">Delete a user</a>.</remarks>
        /// <param name="usernameToMerge">Login name of the user to be merged.</param>
        /// <param name="targetUser">Login name of the user to merge <paramref name="usernameToMerge"/> into.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task MergeUsers(string usernameToMerge, string targetUser);

        /// <summary>
        /// Get all groups the specified user participates in.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/GET-User-Groups.html">Get all groups the specified user participates in</a>.</remarks>
        /// <param name="username">Login name of the user to retrieve information for.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Group" /> instances.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<ICollection<Group>> GetGroupsForUser(string username);

        /// <summary>
        /// Add user to group.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/POST-User-Group.html">Add user account to a group</a>.</remarks>
        /// <param name="username">Login name of the user to be updated.</param>
        /// <param name="group">Name of the group to add the user to.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task AddUserToGroup(string username, string group);

        /// <summary>
        /// Remove user from group.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/DELETE-User-Group.html">Remove user account from a group</a>.</remarks>
        /// <param name="username">Login name of the user to be updated.</param>
        /// <param name="group">Name of the group to remove the user from.</param>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task RemoveUserFromGroup(string username, string group);
    }
}