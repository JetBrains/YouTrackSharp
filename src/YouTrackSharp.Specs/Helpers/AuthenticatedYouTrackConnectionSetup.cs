using Machine.Specifications;

namespace YouTrackSharp.Specs.Helpers
{
    public class AuthenticatedYouTrackConnectionSetup : YouTrackConnectionSetup
    {
        Establish context = () =>
        {
            connection.Authenticate("youtrackapi", "youtrackapi");
        };

    }
}