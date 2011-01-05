using Machine.Specifications;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Specs.Helpers
{
    public class YouTrackConnection
    {
        protected static Connection connection;
        Establish context = () => { connection = new Connection("youtrack.jetbrains.net"); };
    }
}