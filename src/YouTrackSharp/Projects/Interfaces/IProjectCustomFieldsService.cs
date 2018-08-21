using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTrackSharp.Projects.Interfaces
{
    public interface IProjectCustomFieldsService
    {
        Task CreateProjectCustomField(string projectId, CustomField customField);
        Task DeleteProjectCustomField(string projectId, string customFieldName);
        Task<CustomField> GetProjectCustomField(string projectId, string customFieldName);
        Task<ICollection<CustomField>> GetProjectCustomFields(string projectId);
        Task UpdateProjectCustomField(string projectId, CustomField customField);
    }
}