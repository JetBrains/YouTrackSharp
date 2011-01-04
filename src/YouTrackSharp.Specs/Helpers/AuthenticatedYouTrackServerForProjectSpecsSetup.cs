using Machine.Specifications;
using YouTrackSharp.Issues;
using YouTrackSharp.Projects;

namespace YouTrackSharp.Specs.Helpers
{
    public class AuthenticatedYouTrackServerForProjectSpecsSetup : AuthenticatedYouTrackServerSetup
    {
        Establish context = () =>
        {
            youTrackProjects = new YouTrackProjects(youTrackServer);
        };

        protected static YouTrackProjects youTrackProjects;
    }
}