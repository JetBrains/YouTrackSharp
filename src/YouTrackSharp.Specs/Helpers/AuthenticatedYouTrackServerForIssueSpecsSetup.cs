using Machine.Specifications;
using YouTrackSharp.Issues;

namespace YouTrackSharp.Specs.Helpers
{
    public class AuthenticatedYouTrackServerForIssueSpecsSetup : AuthenticatedYouTrackServerSetup
    {
        Establish context = () =>
        {
            IssueManagement = new IssueManagement(youTrackServer);
        };

        protected static IssueManagement IssueManagement;
    }
}