using System;
using System.Collections.Generic;

namespace YouTrackSharp.Projects
{
    public class ProjectManagement
    {
        readonly Connection _connection;

        public ProjectManagement(Connection connection)
        {
            _connection = connection;
        }

        public IList<Project> GetProjects()
        {
            return GetProjectDataByType<MultipleProjectWrapper, Project>("all");
        }

        public IList<ProjectPriority> GetPriorities()
        {
            return GetProjectDataByType<MultipleProjectPriorityWrapper, ProjectPriority>("priorities");
        }

        public IList<ProjectState> GetStates()
        {
            return GetProjectDataByType<MultipleProjectStateWrapper, ProjectState>("states");
        }

        public IList<ProjectIssueTypes> GetIssueTypes()
        {
            return GetProjectDataByType<MultipleProjectIssueTypesWrapper, ProjectIssueTypes>("types");
        }

        IList<TInternal> GetProjectDataByType<TWrapper, TInternal>(string dataType) where TWrapper: IDataWrapper<TInternal>
        {
            var response = _connection.Get<TWrapper>(String.Format("project/{0}", dataType));

            if (response != null)
            {
                return response.Data;
            }
            return new List<TInternal>();
        }

        public IList<ProjectResolutionTypes> GetResolutions()
        {
            return GetProjectDataByType<MultipleProjectResolutionTypesWrapper, ProjectResolutionTypes>("resolutions");
        }
    }
}