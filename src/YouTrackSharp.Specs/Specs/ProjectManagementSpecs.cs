using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using YouTrackSharp.Projects;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{
    [Subject(typeof (ProjectManagement), "given authenticated connection and existing projects")]
    public class when_retrieving_a_list_of_projects : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { projects = projectManagement.GetProjects(); };

        It should_return_all_projects = () => { projects.ShouldNotBeEmpty(); };

        It should_contain_valid_project_data = () => { projects.First().Name.ShouldNotBeEmpty(); };

        static IEnumerable<Project> projects;
    }

    [Subject(typeof (ProjectManagement), "given authenticated connection and existing projects")]
    public class when_retrieving_a_list_of_priorities : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { priorities = projectManagement.GetPriorities(); };

        It should_return_all_priorities = () => priorities.ShouldNotBeEmpty();

        It should_contain_correct_name = () => priorities.First().Name.ShouldNotBeEmpty();

        It should_contain_correct_numericvalue = () => priorities.First().NumericValue.ShouldNotBeEmpty();

        static IEnumerable<ProjectPriority> priorities;
    }

    [Subject(typeof (ProjectManagement), "given authenticated connection and existing projects")]
    public class when_retrieving_a_list_of_states : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { states = projectManagement.GetStates(); };

        It should_return_all_states = () => states.Count().ShouldBeGreaterThan(0);

        It should_contain_valid_state_data = () => states.First().Name.ShouldNotBeEmpty();

        static IEnumerable<ProjectState> states;
    }

    [Subject(typeof (ProjectManagement), "given authenticated connection and existing projects")]
    public class when_retrieving_a_list_of_issue_types : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { issueTypes = projectManagement.GetIssueTypes(); };

        It should_return_all_issue_types = () => issueTypes.ShouldNotBeNull();

        It should_contain_valid_issue_type_data = () => issueTypes.First().Name.ShouldNotBeEmpty();

        static IEnumerable<ProjectIssueTypes> issueTypes;
    }

    [Subject(typeof (ProjectManagement), "given authenticated connection and existing projects")]
    public class when_retrieving_a_list_of_resolution_states : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { resolutions = projectManagement.GetResolutions(); };

        It should_return_all_resolution_states = () => resolutions.ShouldNotBeEmpty();

        It should_contain_valid_resolution_state_data = () => resolutions.First().Name.ShouldNotBeEmpty();

        static IEnumerable<ProjectResolutionTypes> resolutions;
    }

    [Ignore]
    [Subject("Project Management")]
    public class when_retrieving_an_existing_project_by_name : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { project = projectManagement.GetProject("SB"); };

        It should_return_the_specified_project = () => project.Name.ShouldEqual("SB");

        static Project project;
    }
}