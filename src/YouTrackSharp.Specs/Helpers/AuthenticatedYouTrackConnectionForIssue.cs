using Machine.Specifications;
using YouTrackSharp.Issues;

namespace YouTrackSharp.Specs.Helpers
{
    public class AuthenticatedYouTrackConnectionForIssue : AuthenticatedYouTrackConnection
    {
        protected static IssueManagement issueManagement;
        Establish context = () => { issueManagement = new IssueManagement(connection); };
    }
}