#region License
// Distributed under the BSD License
// =================================
// 
// Copyright (c) 2010-2011, Hadi Hariri
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// =============================================================
#endregion
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using YouTrackSharp.Projects;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{
    [Subject(typeof (ProjectManagement))]
    public class when_retrieving_a_list_of_projects_given_authenticated_connection_and_existing_projects : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { projects = projectManagement.GetProjects(); };

        It should_return_all_projects = () => { projects.ShouldNotBeEmpty(); };

        It should_contain_valid_project_data = () => { projects.First().Name.ShouldNotBeEmpty(); };

        static IEnumerable<Project> projects;
    }

    [Subject(typeof (ProjectManagement))]
    public class when_retrieving_a_list_of_priorities_given_authenticated_connection_and_existing_projects : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { priorities = projectManagement.GetPriorities(); };

        It should_return_all_priorities = () => priorities.ShouldNotBeEmpty();

        It should_contain_correct_name = () => priorities.First().Name.ShouldNotBeEmpty();

        It should_contain_correct_numericvalue = () => priorities.First().NumericValue.ShouldNotBeNull();

        static IEnumerable<ProjectPriority> priorities;
    }

    [Subject(typeof (ProjectManagement))]
    public class when_retrieving_a_list_of_states_given_authenticated_connection_and_existing_projects : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { states = projectManagement.GetStates(); };

        It should_return_all_states = () => states.Count().ShouldBeGreaterThan(0);

        It should_contain_valid_state_data = () => states.First().Name.ShouldNotBeEmpty();

        static IEnumerable<ProjectState> states;
    }

    [Subject(typeof (ProjectManagement))]
    public class when_retrieving_a_list_of_issue_types_given_authenticated_connection_and_existing_projects : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { issueTypes = projectManagement.GetIssueTypes(); };

        It should_return_all_issue_types = () => issueTypes.ShouldNotBeNull();

        It should_contain_valid_issue_type_data = () => issueTypes.First().Name.ShouldNotBeEmpty();

        static IEnumerable<ProjectIssueTypes> issueTypes;
    }

    [Subject(typeof (ProjectManagement))]
    public class when_retrieving_a_list_of_resolution_states_given_authenticated_connection_and_existing_projects : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { resolutions = projectManagement.GetResolutions(); };

        It should_return_all_resolution_states = () => resolutions.ShouldNotBeEmpty();

        It should_contain_valid_resolution_state_data = () => resolutions.First().Name.ShouldNotBeEmpty();

        static IEnumerable<ProjectResolutionType> resolutions;
    }

    [Subject(typeof(ProjectManagement))]
    public class when_retrieving_an_existing_project_by_name : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () => { project = projectManagement.GetProject("SB"); };

        It should_return_the_specified_project = () => { project.Name.ShouldEqual("Sandbox"); };

        static Project project;
    }

    [Subject(typeof(ProjectManagement))]
    [Ignore("The server says forbidden.")]
    public class when_retrieving_A_list_of_versions_given_authenticated_connection_and_existing_projects : AuthenticatedYouTrackConnectionForProjectSpecs
    {
        Because of = () =>
            {
                project = projectManagement.GetProject("SB");
                projectManagement.AddVersion(project, new ProjectVersion { Name= "0.1", IsReleased = false, IsArchived = false});
                versions = projectManagement.GetVersions(project);
                projectManagement.DeleteVersion(project, "0.1");
            };

        It should_return_versions = () => { versions.ShouldNotBeEmpty(); };

        static Project project;
        static IEnumerable<ProjectVersion> versions;
    }
    
}