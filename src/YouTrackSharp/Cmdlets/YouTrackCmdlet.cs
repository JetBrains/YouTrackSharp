using System.Management.Automation;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.CmdLets
{
    public class YouTrackCmdlet: PSCmdlet
    {
        protected Connection Connection;
        
        protected override void BeginProcessing()
        {

            Connection = new Connection("youtrack.jetbrains.net");

            Connection.Authenticate("abc", "abc");
        }

    }
}