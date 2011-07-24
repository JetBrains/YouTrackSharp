using CommandLine;

namespace YouTrackSharp.Commands.CommandOptions
{
    public class DeleteIssueCommandOptions
    {
        [Option("i", "issueId", Required = true)] 
        public string IssueId;
    }
}