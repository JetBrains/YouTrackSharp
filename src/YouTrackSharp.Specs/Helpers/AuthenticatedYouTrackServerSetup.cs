using Machine.Specifications;

namespace YouTrackSharp.Specs.Helpers
{
    public class AuthenticatedYouTrackServerSetup : YouTrackServerSetup
    {
        Establish context = () =>
        {
            Server.Login("youtrackapi", "youtrackapi");
        };
    }
}