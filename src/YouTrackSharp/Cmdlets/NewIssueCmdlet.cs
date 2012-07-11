using System.Collections.Generic;
using System.Linq;
using System.Management.Automation;
using YouTrackSharp.Issues;

namespace YouTrackSharp.CmdLets
{
    [Cmdlet(VerbsCommon.New, "issue")]
    public class NewIssueCmdlet : YouTrackIssueCmdlet
    {
        [Parameter(Mandatory = true, HelpMessage = "Summary")]
        [ValidateNotNull]
        public string Summary { get; set; }

        [Parameter(HelpMessage = "Description")]
        public string Description { get; set; }

        [Parameter(HelpMessage = "Priority")]
        public string Priority { get; set; }

        [Parameter(Mandatory = true, HelpMessage = "Project short name")]
        [ValidateNotNull]
        public string ProjectShortName { get; set; }

        [Parameter(HelpMessage = "Priority")]
        public string Type { get; set; }

        [Parameter(HelpMessage = "Subsystem")]
        public string Subsystem { get; set; }

        [Parameter(HelpMessage = "Force issue creation without check")]
        public bool Force { get; set; }

        protected override void ProcessRecord()
        {
            if (!Force)
            {
                var similarIssues =
                    IssueManagement.GetIssuesBySearch(string.Format("project: {0} \"{1}\" \"{2}\"", ProjectShortName, Summary, Description));

                
                var issueList = from si in similarIssues
                                select new {IssueId = si.Id, Summary = si.Summary, Description = si.Description};

                
                if (issueList.Count() > 0)
                {
                    WriteWarning("Found similar issues. If you still want to create it, use -Force=true");
                    WriteObject(issueList);

                    return;
                }
            }
            
            
            var newIssue = new Issue
            {
                Summary = Summary,
                Description = Description,
                Priority  =  Priority,
                ProjectShortName = ProjectShortName,
                ReporterName = Connection.GetCurrentAuthenticatedUser().Username,
                State = "Submitted",
                Type = Type,
                Subsystem = Subsystem
            };

            var id = IssueManagement.CreateIssue(newIssue);

            WriteObject(string.Format("Issue Created with id: {0}", id));
        }
    }
}