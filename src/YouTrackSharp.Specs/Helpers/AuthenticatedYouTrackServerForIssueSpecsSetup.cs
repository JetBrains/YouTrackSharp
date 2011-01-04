using Machine.Specifications;
using YouTrackSharp.Issues;

namespace YouTrackSharp.Specs.Helpers
{
    public class AuthenticatedYouTrackServerForIssueSpecsSetup : AuthenticatedYouTrackServerSetup
    {
        Establish context = () =>
        {
            youTrackIssues = new YouTrackIssues(youTrackServer);
        };

        protected static YouTrackIssues youTrackIssues;
    }
}