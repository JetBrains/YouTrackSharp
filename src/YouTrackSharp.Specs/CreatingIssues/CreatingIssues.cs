using System;
using Machine.Specifications;

namespace YouTrackSharp.Specs.CreatingIssues
{
    [Subject("Creating Issues")]
    public class when_creating_a_new_issue_with_valid_information_and_not_authenticated
    {
        Establish context = () =>
        {
            youTrackConnection = new YouTrackConnection("youtrack.jetbrains.net");

            youTrackIssues = new YouTrackIssues(youTrackConnection);
        };

        Because of = () =>
        {
            var issue = new NewIssue();

            issue.Project = "SB";
            issue.Summary = "Issue Created";

            exception = Catch.Exception(() => { youTrackIssues.CreateIssue(issue); });
        };

        It should_throw_invalid_request_with_message_not_authenticated = () =>
        {
            exception.ShouldBeOfType(typeof(InvalidRequestException));
            exception.Message.ShouldEqual("Not Logged In");
        };

        static YouTrackConnection youTrackConnection;
        static object response;
        static Exception exception;
        static YouTrackIssues youTrackIssues;
    } 
    
    [Subject("Creating Issues")]
    public class when_creating_a_new_issue_with_valid_information_and_authenticated
    {
        Establish context = () =>
        {
            youTrackConnection = new YouTrackConnection("youtrack.jetbrains.net");

            youTrackConnection.Login("youtrackapi", "youtrackapi");

            youTrackIssues = new YouTrackIssues(youTrackConnection);
        };

        Because of = () =>
        {
            var issue = new NewIssue();

            issue.Project = "SB";
            issue.Summary = "Issue Created";

            response  = youTrackIssues.CreateIssue(issue);
        };

        It should_return_issue = () =>
        {

        };

        static YouTrackConnection youTrackConnection;
        static YouTrackIssues youTrackIssues;
        static object response;

    }
}