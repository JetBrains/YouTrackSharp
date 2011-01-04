using System;
using System.Collections.Generic;

namespace YouTrackSharp.Projects
{
    public class YouTrackProjects
    {
        readonly YouTrackServer _youTrackServer;

        public YouTrackProjects(YouTrackServer youTrackServer)
        {
            _youTrackServer = youTrackServer;
        }

        public IList<Project> GetProjects()
        {
            var response = _youTrackServer.Get<MultipleProjectWrapper>("project/all");

            if (response != null)
            {
                return response.project;
            }
            return new List<Project>();
        }

        public IList<ProjectPriority> GetPriorities()
        {
            var response = _youTrackServer.Get<MultipleProjectPriorityWrapper>(String.Format("project/priorities"));

            if (response != null)
            {
                return response.Priority;
            }
            return new List<ProjectPriority>();
        }

        public IList<ProjectState> GetStates()
        {
            var response = _youTrackServer.Get<MultipleProjectStateWrapper>(String.Format("project/states"));

            if (response != null)
            {
                return response.State;
            }
            return new List<ProjectState>();
        }

        public IList<ProjectIssueTypes> GetIssueTypes()
        {
            var response = _youTrackServer.Get<MultipleProjectIssueTypesWrapper>(String.Format("project/types"));

            if (response != null)
            {
                return response.Type;
            }
            return new List<ProjectIssueTypes>();
        }
    }
}