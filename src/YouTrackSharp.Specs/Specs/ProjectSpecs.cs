using System.Collections.Generic;
using Machine.Specifications;
using YouTrackSharp.Projects;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{

    [Subject("Projects")]
    public class when_retrieving_a_list_of_existing_projects: AuthenticatedYouTrackServerForProjectSpecsSetup
    {
        Because of = () =>
        {
            projects = youTrackProjects.GetProjects();

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

    [Subject("Projects")]
    public class when_retrieving_a_list_of_existing_priorities: AuthenticatedYouTrackServerForProjectSpecsSetup
    {
        Because of = () =>
        {
            priorities = youTrackProjects.GetPriorities();

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

    [Subject("Projects")]
    public class when_retrieving_a_list_of_existing_states: AuthenticatedYouTrackServerForProjectSpecsSetup
    {
        Because of = () =>
        {
            states = youTrackProjects.GetStates();

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

    [Subject("Projects")]
    public class when_retrieving_a_list_of_existing_issue_types: AuthenticatedYouTrackServerForProjectSpecsSetup
    {
        Because of = () =>
        {
            issueTypes = youTrackProjects.GetIssueTypes();

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

}