using System;
using Machine.Specifications;

namespace YouTrackSharp.Specs.CreatingIssues
{
    [Subject("Creating Issues")]
    public class when_creating_a_new_issue_with_valid_information_and_not_authenticated
    {
        Establish context = () =>
        {
            youTrackClient = new YouTrackClient("youtrack.jetbrains.net");
        };

        Because of = () =>
        {
            var issue = new NewIssue();

            issue.Project = "SB";
            issue.Summary = "Issue Created";

            exception = Catch.Exception(() => { youTrackClient.CreateIssue(issue); });
        };

        It should_throw_invalid_request_with_message_not_authenticated = () =>
        {
            exception.ShouldBeOfType(typeof(InvalidRequestException));
            exception.Message.ShouldEqual("Not Logged In");
        };

        static YouTrackClient youTrackClient;
        static object response;
        static Exception exception;

    } 
    
    [Subject("Creating Issues")]
    public class when_creating_a_new_issue_with_valid_information_and_authenticated
    {
        Establish context = () =>
        {
            youTrackClient = new YouTrackClient("youtrack.jetbrains.net");

            youTrackClient.Login("youtrackapi", "youtrackapi");
        };

        Because of = () =>
        {
            var issue = new NewIssue();

            issue.Project = "SB";
            issue.Summary = "Issue Created";

            response  = youTrackClient.CreateIssue(issue);
        };

        It should_return_issue = () =>
        {

        };

        static YouTrackClient youTrackClient;
        static object response;

    }
}