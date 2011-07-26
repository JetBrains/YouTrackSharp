using YouTrackSharp.Issues;

namespace YouTrackSharp.CmdLets
{
    public class YouTrackIssueCmdlet: YouTrackCmdlet
    {
        protected IssueManagement IssueManagement;

        protected override void BeginProcessing()
        {
            base.BeginProcessing();

            IssueManagement = new IssueManagement(Connection);
        }
    }
}