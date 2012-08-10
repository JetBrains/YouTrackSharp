#region License

// Distributed under the BSD License
//   
// YouTrackSharp Copyright (c) 2010-2012, Hadi Hariri and Contributors
// All rights reserved.
//   
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//      * Redistributions of source code must retain the above copyright
//         notice, this list of conditions and the following disclaimer.
//      * Redistributions in binary form must reproduce the above copyright
//         notice, this list of conditions and the following disclaimer in the
//         documentation and/or other materials provided with the distribution.
//      * Neither the name of Hadi Hariri nor the
//         names of its contributors may be used to endorse or promote products
//         derived from this software without specific prior written permission.
//   
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
//   TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
//   PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
//   <COPYRIGHTHOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//   SPECIAL,EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//   LIMITED  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//   DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  ON ANY
//   THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//   THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//   

#endregion

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

                // TODO: Fix this once issues work again with dynamic type
                //var issueList = from si in similarIssues
                //                select new {IssueId = si.Id, Summary = si.Summary, Description = si.Description};


                //if (issueList.Count() > 0)
                //{
                //    WriteWarning("Found similar issues. If you still want to create it, use -Force=true");
                //    WriteObject(issueList);

                //    return;
                //}
            }


            dynamic newIssue = new Issue();

            newIssue.Summary = Summary;
            newIssue.Description = Description;
            newIssue.Priority = new[] {Priority};
            newIssue.ProjectShortName = ProjectShortName;
            newIssue.ReporterName = Connection.GetCurrentAuthenticatedUser().Username;
            newIssue.State = "Submitted";
            newIssue.Type = Type;
            newIssue.Subsystem = Subsystem;

            var id = IssueManagement.CreateIssue(newIssue);

            WriteObject(string.Format("Issue Created with id: {0}", id));
        }
    }
}