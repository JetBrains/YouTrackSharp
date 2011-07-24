using CommandLine;

namespace YouTrackSharp.Commands
{
    public class CreateIssueCommandOptions
    {
        [Option("p", "project", Required = true)]
        public string ProjectName;
        [Option("t", "type")]
        public string Type;
        [Option("s", "summmary", Required = true)]
        public string Summary;
        [Option("d", "description", Required = true)]
        public string Description;
        [Option("y", "subsystem")]
        public string SubSystem;
        [Option("a", "assignee")]
        public string Assignee;
        [Option("x", "priority")]
        public string Priority;
    }
}