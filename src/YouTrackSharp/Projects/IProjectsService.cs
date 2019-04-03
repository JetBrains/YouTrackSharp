using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTrackSharp.Projects
{
    public interface IProjectsService
    {
        /// <summary>
        /// Get a list of all accessible projects from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Accessible-Projects.html">Get Accessible Projects</a>.</remarks>
        /// <param name="verbose">If full representation of projects is returned. If this parameter is false, only short names and id's are returned.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Project" /> that are accessible for currently logged in user.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<ICollection<Project>> GetAccessibleProjects(bool verbose = false);
    }
}