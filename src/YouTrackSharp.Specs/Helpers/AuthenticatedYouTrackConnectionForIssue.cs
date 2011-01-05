using Machine.Specifications;
using YouTrackSharp.Issues;

namespace YouTrackSharp.Specs.Helpers
{
    public class AuthenticatedYouTrackConnectionForIssue : AuthenticatedYouTrackConnection
    {
        Establish context = () =>
        {
            issueManagement = new IssueManagement(connection);
        };

        protected static IssueManagement issueManagement;
    }
}