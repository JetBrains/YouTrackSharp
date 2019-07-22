using System.Threading.Tasks;
using JetBrains.Annotations;

namespace YouTrackSharp.Users
{
    [PublicAPI]
    public interface IUserService
    {
        /// <summary>
        /// Get info about currently logged in user.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Info-For-Current-User.html">Get Info For Current User</a>.</remarks>
        /// <returns>A <see cref="User" /> instance that represents the currently logged in user.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<User> GetCurrentUserInfo();
    }
}