using System.Management.Automation;

namespace YouTrackSharp.CmdLets
{
    [Cmdlet(VerbsCommon.Set, "command")]
    public class SetCommandCmdlet : YouTrackIssueCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Command to apply")]
        [ValidateNotNull]
        public string Command { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Issue Id")]
        [ValidateNotNull]
        public string IssueId { get; set; }

        [Parameter(HelpMessage = "Comment")]
        public string Comment { get; set; }

        protected override void ProcessRecord()
        {
            IssueManagement.ApplyCommand(IssueId, Command, Comment);
        }
    }
}