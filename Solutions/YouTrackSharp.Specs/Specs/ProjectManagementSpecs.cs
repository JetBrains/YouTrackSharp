#region Settings
#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
#endregion

namespace YouTrackSharp.Specs.Specs
{
    #region Using Directives

    using System.Collections.Generic;
    using System.Linq;
    using Machine.Specifications;
    using YouTrackSharp.Projects;
    using YouTrackSharp.Specs.Helpers;

    #endregion

    [Subject("Project Management")]
    public class when_retrieving_a_list_of_existing_projects: AuthenticatedYouTrackConnectionForProjectSpecsSetup
    {
        Because of = () =>
        {
            projects = projectManagement.GetProjects();

        };

        It should_return_all_projects = () =>
        {
            projects.Count().ShouldBeGreaterThan(0);
        };

        It should_contain_valid_project_data = () =>
        {
            projects.First().Name.ShouldNotBeEmpty();
        };

        static IEnumerable<Project> projects;
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
            priorities.Count().ShouldBeGreaterThan(0);
        };
        
        It should_contain_valid_priority_data = () =>
        {
            priorities.First().Name.ShouldNotBeEmpty();
            priorities.First().NumericValue.ShouldNotBeEmpty();
        };
        static IEnumerable<ProjectPriority> priorities;
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
            states.Count().ShouldBeGreaterThan(0);
        };
        
        It should_contain_valid_state_data = () =>
        {
            states.First().Name.ShouldNotBeEmpty();
        };

        static IEnumerable<ProjectState> states;
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
            issueTypes.Count().ShouldBeGreaterThan(0);
        };
        
        It should_contain_valid_issue_type_data = () =>
        {
            issueTypes.First().Name.ShouldNotBeEmpty();
        };

        static IEnumerable<ProjectIssueTypes> issueTypes;
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
            resolutions.Count().ShouldBeGreaterThan(0);
        };
        
        It should_contain_valid_resolution_state_data = () =>
        {
            resolutions.First().Name.ShouldNotBeEmpty();
        };

        static IEnumerable<ProjectResolutionTypes> resolutions;
    }

    [Ignore]
    [Subject("Project Management")]
    public class when_retrieving_an_existing_project_by_name: AuthenticatedYouTrackConnectionForProjectSpecsSetup
    {
        Because of = () =>
        {
            project = projectManagement.GetProject("SB");

        };

        It should_return_the_specified_project = () =>
        {
            project.Name.ShouldEqual("SB");
        };

        static Project project;
    }

}