using System;
using Machine.Specifications;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;

namespace YouTrackSharp.Specs.Issues.CreatingIssues
{
    [Subject("Creating Issues")]
    public class when_creating_a_new_issue_with_valid_information_and_not_authenticated
    {
        Establish context = () =>
        {
            youTrackServer = new YouTrackServer("youtrack.jetbrains.net");

            youTrackIssues = new YouTrackIssues(youTrackServer);
        };

        Because of = () =>
        {
            var issue = new NewIssueMessage();

            issue.Project = "SB";
            issue.Summary = "Issue Created";

            exception = Catch.Exception(() => { youTrackIssues.CreateIssue(issue); });
        };

        It should_throw_invalid_request_with_message_not_authenticated = () =>
        {
            exception.ShouldBeOfType(typeof(InvalidRequestException));
            exception.Message.ShouldEqual("Not Logged In");
        };

        static YouTrackServer youTrackServer;
        static object response;
        static Exception exception;
        static YouTrackIssues youTrackIssues;
    } 
    
    [Subject("Creating Issues")]
    public class when_creating_a_new_issue_with_valid_information_and_authenticated
    {
        Establish context = () =>
        {
            youTrackServer = new YouTrackServer("youtrack.jetbrains.net");

            youTrackServer.Login("youtrackapi", "youtrackapi");

            youTrackIssues = new YouTrackIssues(youTrackServer);
        };

        Because of = () =>
        {
            var issue = new NewIssueMessage
                        {
                            Project = "SB",
                            Summary = "Issue Created",
                            Assignee = "youtrackapi",
                            Description = "Some description",
                            FixedInBuild = "1.0",
                            Priority = "1",
                            ReporterName = "youtrackapi",
                            State = "Submitted",
                            Subsystem = "Unassigned",
                            Type = "Bug"
                        };


            response  = youTrackIssues.CreateIssue(issue);
        };

        It should_return_issue = () =>
        {

        };

        static YouTrackServer youTrackServer;
        static YouTrackIssues youTrackIssues;
        static object response;

    }
}