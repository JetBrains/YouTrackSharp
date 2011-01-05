using Machine.Specifications;
using YouTrackSharp.Issues;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Specs.Helpers
{
    public class AuthenticatedYouTrackConnectionForProjectSpecs : AuthenticatedYouTrackConnection
    {
        Establish context = () =>
        {
            projectManagement = new ProjectManagement(connection);
        };

        protected static ProjectManagement projectManagement;
    }
}