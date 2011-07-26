using System.Management.Automation;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.CmdLets
{
    public class YouTrackCmdlet: PSCmdlet
    {
        protected Connection Connection;

        protected override void BeginProcessing()
        {
            // Sample for now...credentials will move out (obviously)
            Connection = new Connection("youtrack.jetbrains.net");

            Connection.Authenticate("youtrackapi", "youtrackapi");
        }

    }
}