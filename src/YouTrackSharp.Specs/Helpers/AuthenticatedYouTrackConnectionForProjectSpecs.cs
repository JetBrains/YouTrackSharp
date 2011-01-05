using Machine.Specifications;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Specs.Helpers
{
    public class AuthenticatedYouTrackConnectionForProjectSpecs : AuthenticatedYouTrackConnection
    {
        protected static ProjectManagement projectManagement;
        Establish context = () => { projectManagement = new ProjectManagement(connection); };
    }
}