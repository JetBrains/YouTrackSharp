using Machine.Specifications;
using YouTrackSharp.Infrastructure;

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