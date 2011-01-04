using Machine.Specifications;
using YouTrackSharp.Server;

namespace YouTrackSharp.Specs.Helpers
{
    public class YouTrackConnectionSetup
    {
        Establish context = () =>
        {
            connection = new Connection("youtrack.jetbrains.net");
        };

        protected static Connection connection;
    }
}