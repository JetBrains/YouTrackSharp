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
            youtrack = new YouTrackClient("youtrack.jetbrains.net");
        };

        Because of = () =>
        {

            issues = youtrack.GetIssues("DCVR", 10);
        };

        It should_return_list_of_issues_for_that_project = () =>
        {
            issues.ShouldNotBeNull();
            issues.Count.ShouldEqual(10);
        };

        static YouTrackClient youtrack;
        static IList<Issue> issues;
    }

    [Subject("Retrieving issues")]
    public class when_requesting_a_specific_issues_that_exists
    {
        Establish context = () =>
        {
            youTrack = new YouTrackClient("youtrack.jetbrains.net");
        };

        Because of = () =>
        {
            issue = youTrack.GetIssue("RSRP-198145");

        };

        It should_return_the_issue = () =>
        {
            issue.ShouldNotBeNull();
            issue.Id.ShouldEqual("RSRP-198145");
            issue.ProjectShortName.ShouldEqual("RSRP");
        };

        static Issue issue;
        static YouTrackClient youTrack;
    }

    [Subject("Retrieving issues")]
    public class when_requesting_a_specific_issues_that_does_not_exist
    {
        Establish context = () =>
        {
            youTrack = new YouTrackClient("youtrack.jetbrains.net");
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => youTrack.GetIssue("fdfdfsdfsd"));

        };

        It should_throw_issue_not_found_exception = () =>
        {
            exception.ShouldBeOfType(typeof(InvalidRequestException));
        }; 

        It should_contain_inner_exception_with_404 = () =>
        {
            var innerException = exception.InnerException;
            innerException.ShouldBeOfType(typeof(HttpException));
            ((HttpException) innerException).StatusCode.ShouldEqual(HttpStatusCode.NotFound);

        };
        
        static YouTrackClient youTrack;
        static Exception exception;
    }
}