using Machine.Specifications;

namespace YouTrackSharp.Specs.Helpers
{
    public class AuthenticatedYouTrackConnection : YouTrackConnection
    {
        Establish context = () => connection.Authenticate("youtrackapi", "youtrackapi");
    }
}