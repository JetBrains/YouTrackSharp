using Machine.Specifications;

namespace YouTrackSharp.Specs.Helpers
{
    public class YouTrackServerSetup
    {
        Establish context = () =>
        {
            Server = new Server("youtrack.jetbrains.net");
        };

        protected static Server Server;
    }
}