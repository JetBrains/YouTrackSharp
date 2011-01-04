using System;
using System.Collections.Generic;
using System.Net;
using EasyHttp.Infrastructure;
using Machine.Specifications;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{
    [Subject("Issue Management")]
    public class when_requesting_list_of_issues_for_project: AuthenticatedYouTrackConnectionForIssueSpecsSetup
    {
     
        Because of = () =>
        {

            issues = issueManagement.GetIssues("SB", 10);
        };

        It should_return_list_of_issues_for_that_project = () =>
        {
            issues.ShouldNotBeNull();
            issues.Count.ShouldEqual(10);
        };

        protected static IList<Issue> issues;
    }

    [Subject("Issue Management")]
    public class when_requesting_a_specific_issues_that_exists: AuthenticatedYouTrackConnectionForIssueSpecsSetup
    {

        Because of = () =>
        {
            issue = issueManagement.GetIssue("SB-282");

        };

        It should_return_the_issue = () =>
        {
            issue.ShouldNotBeNull();
            issue.Id.ShouldEqual("SB-282");
            issue.ProjectShortName.ShouldEqual("SB");
        };

        static Issue issue;
    }

    [Subject("Issue Management")]
    public class when_requesting_a_specific_issues_that_does_not_exist: AuthenticatedYouTrackConnectionForIssueSpecsSetup
    {
        Because of = () =>
        {
            exception = Catch.Exception(() => issueManagement.GetIssue("fdfdfsdfsd"));

        };

        It should_throw_invalid_request_exception = () =>
        {
            exception.ShouldBeOfType(typeof(InvalidRequestException));
        };

        It should_contain_inner_exception_of_type_http_exception = () =>
        {
            innerException = exception.InnerException;
            innerException.ShouldBeOfType(typeof(HttpException));
        };

        It inner_http_exception_should_contain_status_code_of_not_found = () =>
        {
            ((HttpException)innerException).StatusCode.ShouldEqual(HttpStatusCode.NotFound);

        };

        static Exception exception;
        static Exception innerException;
    }

    [Subject("Issue Management")]
    public class when_retrieving_comments_of_an_existing_issue_that_has_comments: AuthenticatedYouTrackConnectionForIssueSpecsSetup
    {
        Because of = () =>
        {
            comments = issueManagement.GetCommentsForIssue("SB-560");

        };

        It should_return_the_comments = () =>
        {
            comments.ShouldNotBeNull();
        };

        static IList<Comment> comments;
    }


    [Subject("Issue Management")]
    public class when_creating_a_new_issue_with_valid_information_and_not_authenticated : YouTrackConnectionSetup
    {

        Establish context = () =>
        {
            IssueManagement = new IssueManagement(connection);

        };

        Because of = () =>
        {
            var issue = new NewIssueMessage();

            issue.Project = "SB";
            issue.Summary = "Issue Created";


            exception = Catch.Exception(() => { IssueManagement.CreateIssue(issue); });
        };

        It should_throw_invalid_request_with_message_not_authenticated = () =>
        {
            exception.ShouldBeOfType(typeof(InvalidRequestException));
            exception.Message.ShouldEqual("Not Logged In");
        };

        protected static IssueManagement IssueManagement;
        static object response;
        static Exception exception;
    }

    [Subject("Issue Management")]
    public class when_creating_a_new_issue_with_valid_information_and_authenticated: AuthenticatedYouTrackConnectionForIssueSpecsSetup
    {
        Because of = () =>
        {
            var issue = new NewIssueMessage
                        {
                            Project = "SB",
                            Summary = "Issue Created",
                      //      Assignee = "youtrackapi",
                            Description = "Some description",
                        //    FixedInBuild = "1.0",
                          //  Priority = "1",
                            //ReporterName = "youtrackapi",
                            //State = "Submitted",
                           // Subsystem = "Unassigned"
                            //Type = "Bug"
                        };
           

            response  = issueManagement.CreateIssue(issue);
        };

        It should_return_issue = () =>
        {

        };
        
        static object response;

    }
}