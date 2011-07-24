using CommandLine;

namespace YouTrackSharp.Commands.CommandOptions
{
    public class ApplyCommandCommandOptions
    {
        [Option("c", "command", Required = true)]
        public string Command;
    }
}