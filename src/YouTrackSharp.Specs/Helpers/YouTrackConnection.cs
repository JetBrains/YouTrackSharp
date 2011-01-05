using Machine.Specifications;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Specs.Helpers
{
    public class YouTrackConnection
    {
        Establish context = () =>
        {
            connection = new Connection("youtrack.jetbrains.net");
        };

        protected static Connection connection;
    }
}