using System;
using System.IO;
using System.Linq;
using System.Reflection;
using CommandLine;

namespace YouTrackSharp.Commands
{
    public class CommandParser
    {
        public Command Parse(string inputString)
        {
            if (String.IsNullOrEmpty(inputString))
            {
                throw new CommandException("Emtpy command");
            }

            var commandAndParams = (from words in inputString
                    let split = inputString.Split(' ')
                    select new { Name = split.First(), Parameters = split.Skip(1) }).FirstOrDefault();

            if (commandAndParams == null)
            {
                throw new CommandException("Invalid Command");
            }

            var commandOptionsType =
                 Assembly.GetExecutingAssembly().GetType(string.Format("{0}.{1}.{2}CommandOptions", Assembly.GetExecutingAssembly().GetName().Name, "Commands.CommandOptions", commandAndParams.Name));
             
            if (commandOptionsType == null)
            {
                throw new CommandException("Invalid Command");
            }

            var commandParameters = Activator.CreateInstance(commandOptionsType);

            ICommandLineParser parser = new CommandLineParser(new CommandLineParserSettings(false));

            if (parser.ParseArguments(commandAndParams.Parameters.ToArray(), commandParameters))
            {
                return new Command() { Name = commandAndParams.Name, Parameters = commandParameters };
            }
            throw new CommandException("Invalid Command Parameters");
        }
    }
}