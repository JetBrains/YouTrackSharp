using System;
using System.Collections.Generic;
using YouTrackSharp.Server;

namespace YouTrackSharp.Projects
{
    public class ProjectManagement
    {
        readonly Connection _connection;

        public ProjectManagement(Connection connection)
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
    }
}