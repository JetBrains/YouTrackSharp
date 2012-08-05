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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using EasyHttp.Infrastructure;
using Machine.Specifications;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{
    [Subject(typeof (IssueManagement))]
    public class when_requesting_list_of_issues_for_a_project_given_authenticated_connection : AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () => { issues = issueManagement.GetAllIssuesForProject("SB", 10); };

        It should_return_list_of_issues_for_that_project = () => issues.ShouldNotBeNull();



        protected static IEnumerable<Issue> issues;
    }

    [Subject(typeof (IssueManagement))]
    public class when_requesting_a_specific_issue_given_authenticated_connection : AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () => { issue = issueManagement.GetIssue("SB-1"); };

        It should_return_issue_with_correct_id = () => issue.Id.ShouldEqual("SB-1");

        It should_return_issue_with_correct_project_name = () => issue.ProjectShortName.ShouldEqual("SB");


        static Issue issue;
    }

    [Subject(typeof (IssueManagement))]
    public class when_requesting_a_specific_issues_that_does_not_exist_given_authenticated_connection : AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () => { exception = Catch.Exception(() => issueManagement.GetIssue("fdfdfsdfsd")); };

        It should_throw_invalid_request_exception = () => exception.ShouldBeOfType<InvalidRequestException>();

        It should_contain_inner_exception_of_type_http_exception =
            () => exception.InnerException.ShouldBeOfType<HttpException>();

        It inner_http_exception_should_contain_status_code_of_not_found =
            () => ((HttpException) exception.InnerException).StatusCode.ShouldEqual(HttpStatusCode.NotFound);

        static Exception exception;
    }

    [Subject(typeof (IssueManagement))]
    public class when_retrieving_comments_of_issue_that_has_comments_given_authenticated_connection : AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () => { comments = issueManagement.GetCommentsForIssue("SB-1"); };

        It should_return_the_comments = () => comments.First().Text.ShouldNotBeEmpty();

        static IEnumerable<Comment> comments;
    }


    [Subject(typeof (IssueManagement))]
    public class when_creating_a_new_issue_with_valid_information_given_a_non_authenticated_connection : YouTrackConnection
    {
        Establish context = () => { IssueManagement = new IssueManagement(connection); };

        Because of = () =>
        {
            var issue = new Issue {ProjectShortName = "SB", Summary = "Issue Created"};


            exception = Catch.Exception(() => { IssueManagement.CreateIssue(issue); });
        };

        It should_throw_invalid_request_with_message_not_authenticated =
            () => exception.ShouldBeOfType(typeof (InvalidRequestException));

        It should_contain_message_not_logged_in = () => exception.Message.ShouldEqual("Not Logged In");

        protected static IssueManagement IssueManagement;
        static object response;
        static Exception exception;
    }

    [Subject(typeof (IssueManagement))]
    public class when_creating_a_new_issue_with_valid_information_and_authenticated :
        AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () =>
        {
            var issue = new Issue
                        {
                            ProjectShortName = "SB",
                            Summary = "something new",
                            Description = "somethingelse new too",
                            AssigneeName = "youtrackapi",
                            Priority = new[] {"Minor"} ,
                            Type = "Task",
                            Subsystem = "Machine Specs",
                            State = "New"
                        };

            string issueId = issueManagement.CreateIssue(issue);
            newIssue = issueManagement.GetIssue(issueId);
        };

        It should_return_issue = () => newIssue.ShouldNotBeNull();

        It should_have_assignee_name = () => newIssue.AssigneeName.ShouldBeEqualIgnoringCase("youtrackapi");
        It should_have_created = () => newIssue.Created.ShouldNotBeNull();
        It should_have_description = () => newIssue.Description = "somethingelse new too";
        It should_have_id = () => newIssue.Id.ShouldNotBeNull();
        It should_have_project_short_name = () => newIssue.ProjectShortName.ShouldBeEqualIgnoringCase("SB");
        It should_have_reporter_name = () => newIssue.ReporterName.ShouldBeEqualIgnoringCase("youtrackapi");
        It should_have_summary = () => newIssue.Summary.ShouldBeEqualIgnoringCase("something new");
        It should_have_priority = () => newIssue.Priority[0].ShouldBeEqualIgnoringCase("Minor");
        It should_have_type = () => newIssue.Type.ShouldBeEqualIgnoringCase("Task");
        It should_have_subsystem = () => newIssue.Subsystem.ShouldBeEqualIgnoringCase("Machine Specs");
        It should_have_state = () => newIssue.State.ShouldBeEqualIgnoringCase("New");

        static Issue newIssue;
    }

    [Subject(typeof (IssueManagement))]
    public class when_checking_to_see_if_an_existing_issue_exists: AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () =>
        {
            result = issueManagement.CheckIfIssueExists("SB-1");

        };

        It should_return_true = () => result.ShouldBeTrue();

        static bool result;
    }

    [Subject(typeof (IssueManagement))]
    public class when_adding_an_attachment_to_an_existing_issue: AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () =>
        {
            issueManagement.AttachFileToIssue("SB-1", @"Helpers\TestAtt.txt");
        };

        It should_be_successful = () =>
        {

        };
    }

    [Subject(typeof (IssueManagement))]
    public class when_applying_a_command_to_an_existing_issue : AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () =>
        {
            issueManagement.ApplyCommand("SB-1", "Fixed", "");

        };

        It should_be_successful = () =>
        {

        };

    }

    [Subject(typeof (IssueManagement))]
    public class when_searching_for_an_issue_given_some_text : AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () =>
        {
            issues = issueManagement.GetIssuesBySearch("some new issue", 100);
        };

        It should_return_list_of_issues = () =>
        {
            issues.ShouldNotBeNull();
        };

        static IEnumerable<Issue> issues;
    }

    [Subject(typeof(IssueManagement))]
    public class when_getting_issue_count_by_search_string : AuthenticatedYouTrackConnectionForIssue
    {
        Because of = () =>
        {
            count = issueManagement.GetIssueCount("some new issue");
        };

        It should_return_number_of_matching_issues = () =>
        {
            count.ShouldBeGreaterThan(0);
        };

        static int count;
    }

}