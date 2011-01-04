using Machine.Specifications;

namespace YouTrackSharp.Specs.Helpers
{
    public class YouTrackServerSetup
    {
        Establish context = () =>
        {
            youTrackServer = new YouTrackServer("youtrack.jetbrains.net");
        };

        protected static YouTrackServer youTrackServer;
    }
}