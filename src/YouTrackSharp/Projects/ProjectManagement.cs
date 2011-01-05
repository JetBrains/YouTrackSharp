using System;
using System.Collections.Generic;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Server;

namespace YouTrackSharp.Projects
{
    public class ProjectManagement
    {
        readonly IConnection _connection;

        public ProjectManagement(IConnection connection)
        {
            _connection = connection;
        }

        public IEnumerable<Project> GetProjects()
        {
            return _connection.Get<MultipleProjectWrapper, Project>("project/all");
        }


        public IEnumerable<ProjectPriority> GetPriorities()
        {
            return _connection.Get<MultipleProjectPriorityWrapper, ProjectPriority>("project/priorities");
        }

        public IEnumerable<ProjectState> GetStates()
        {
            return _connection.Get<MultipleProjectStateWrapper, ProjectState>("project/states");
        }

        public IEnumerable<ProjectIssueTypes> GetIssueTypes()
        {
            return _connection.Get<MultipleProjectIssueTypesWrapper, ProjectIssueTypes>("project/types");
        }

        public IEnumerable<ProjectResolutionTypes> GetResolutions()
        {
            return _connection.Get<MultipleProjectResolutionTypesWrapper, ProjectResolutionTypes>("project/resolutions");
        }

        public Project GetProject(string projectName)
        {
            return _connection.Get<Project>(String.Format("admin/project/{0}", projectName));
        }
    }
}