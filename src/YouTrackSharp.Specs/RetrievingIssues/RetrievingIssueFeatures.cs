using System;
using System.Collections.Generic;
using System.Net;
using EasyHttp.Http;
using Machine.Specifications;

namespace YouTrackSharp.Specs.RetrievingIssues
{
    [Subject("Retrieving Issues")]
    public class when_requesting_list_of_issues_for_project
    {
        Establish context = () =>
        {
            youtrackConnection = new YouTrackConnection("youtrack.jetbrains.net");

            youtrackConnection.Login("youtrackapi", "youtrackapi");
            
            youTrackIssues = new YouTrackIssues(youtrackConnection);
        };

        Because of = () =>
        {

            issues = youTrackIssues.GetIssues("SB", 10);
        };

        It should_return_list_of_issues_for_that_project = () =>
        {
            issues.ShouldNotBeNull();
            issues.Count.ShouldEqual(10);
        };

        static YouTrackConnection youtrackConnection;
        static IList<Issue> issues;
        static YouTrackIssues youTrackIssues;
    }

    [Subject("Retrieving issues")]
    public class when_requesting_a_specific_issues_that_exists
    {
        Establish context = () =>
        {
            youTrackConnection = new YouTrackConnection("youtrack.jetbrains.net");

            youTrackConnection.Login("youtrackapi", "youtrackapi");

            youTrackIssues = new YouTrackIssues(youTrackConnection);
        };

        Because of = () =>
        {
            issue = youTrackIssues.GetIssue("SB-282");

        };

        It should_return_the_issue = () =>
        {
            issue.ShouldNotBeNull();
            issue.Id.ShouldEqual("SB-282");
            issue.ProjectShortName.ShouldEqual("SB");
        };

        static Issue issue;
        static YouTrackConnection youTrackConnection;
        static YouTrackIssues youTrackIssues;
    }

    [Subject("Retrieving issues")]
    public class when_requesting_a_specific_issues_that_does_not_exist
    {
        Establish context = () =>
        {
            youTrackConnection = new YouTrackConnection("youtrack.jetbrains.net");

            youTrackIssues = new YouTrackIssues(youTrackConnection);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => youTrackIssues.GetIssue("fdfdfsdfsd"));

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
            ((HttpException) innerException).StatusCode.ShouldEqual(HttpStatusCode.NotFound);

        };

        static YouTrackConnection youTrackConnection;
        static YouTrackIssues youTrackIssues;
        static Exception exception;
        static Exception innerException;
    }
}