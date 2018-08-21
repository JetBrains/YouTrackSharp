using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTrackSharp.Projects.Interfaces
{
    public interface IProjectsService
    {
        Task<ICollection<Project>> GetAccessibleProjects(bool verbose = false);
    }
}