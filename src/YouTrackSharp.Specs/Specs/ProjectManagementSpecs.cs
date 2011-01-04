using System.Collections.Generic;
using Machine.Specifications;
using YouTrackSharp.Projects;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{

    [Subject("Project Management")]
    public class when_retrieving_a_list_of_existing_projects: AuthenticatedYouTrackConnectionForProjectSpecsSetup
    {
        Because of = () =>
        {
            projects = projectManagement.GetProjects();

        };

        It should_return_all_projects = () =>
        {
            projects.Count.ShouldBeGreaterThan(0);
        };

        It should_contain_valid_project_data = () =>
        {
            projects[0].Name.ShouldNotBeEmpty();
        };

        static IList<Project> projects;
    }

    [Subject("Project Management")]
    public class when_retrieving_a_list_of_existing_priorities: AuthenticatedYouTrackConnectionForProjectSpecsSetup
    {
        Because of = () =>
        {
            priorities = projectManagement.GetPriorities();

        };

        It should_return_all_priorities = () =>
        {
            priorities.Count.ShouldBeGreaterThan(0);
        };
        
        It should_contain_valid_priority_data = () =>
        {
            priorities[0].Name.ShouldNotBeEmpty();
            priorities[0].NumericValue.ShouldNotBeEmpty();
        };
        static IList<ProjectPriority> priorities;
    }

    [Subject("Project Management")]
    public class when_retrieving_a_list_of_existing_states: AuthenticatedYouTrackConnectionForProjectSpecsSetup
    {
        Because of = () =>
        {
            states = projectManagement.GetStates();

        };

        It should_return_all_states = () =>
        {
            states.Count.ShouldBeGreaterThan(0);
        };
        
        It should_contain_valid_state_data = () =>
        {
            states[0].Name.ShouldNotBeEmpty();
        };

        static IList<ProjectState> states;
    }

    [Subject("Project Management")]
    public class when_retrieving_a_list_of_existing_issue_types: AuthenticatedYouTrackConnectionForProjectSpecsSetup
    {
        Because of = () =>
        {
            issueTypes = projectManagement.GetIssueTypes();

        };

        It should_return_all_issue_types = () =>
        {
            issueTypes.Count.ShouldBeGreaterThan(0);
        };
        
        It should_contain_valid_issue_type_data = () =>
        {
            issueTypes[0].Name.ShouldNotBeEmpty();
        };

        static IList<ProjectIssueTypes> issueTypes;
    }

    [Subject("Project Management")]
    public class when_retrieving_a_list_of_resolution_states: AuthenticatedYouTrackConnectionForProjectSpecsSetup
    {
        Because of = () =>
        {
            resolutions = projectManagement.GetResolutions();

        };

        It should_return_all_resolution_states = () =>
        {
            resolutions.Count.ShouldBeGreaterThan(0);
        };
        
        It should_contain_valid_resolution_state_data = () =>
        {
            resolutions[0].Name.ShouldNotBeEmpty();
        };

        static IList<ProjectResolutionTypes> resolutions;
    }

}