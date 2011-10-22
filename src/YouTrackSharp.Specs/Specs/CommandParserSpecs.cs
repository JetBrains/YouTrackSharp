using System;
using Machine.Specifications;
using YouTrackSharp.Commands;
using YouTrackSharp.Commands.CommandOptions;

namespace YouTrackSharp.Specs.Specs
{
    [Subject(typeof (CommandParser))]
    public class when_parsing_an_invalid_command
    {
        Establish context = () =>
        {
            commandParser = new CommandParser();
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => commandParser.Parse("blabla"));
        };

        It should_throw_command_exception = () => exception.ShouldBeOfType<CommandException>();

        It should_contain_message_indicating_invalid_command = () => exception.Message.ShouldEqual("Invalid Command");

        static CommandParser commandParser;
        static Exception exception;
    }

    [Subject(typeof (CommandParser))]
    public class when_parsing_a_valid_command
    {
        Establish context = () =>
        {
            commandParser = new CommandParser();
        };

        Because of = () => command = commandParser.Parse("CreateIssue -p RSRP -s \"Summary\" -d \"Description\"");

        It should_return_a_command_with_command_name_set = () => command.Name.ShouldEqual("CreateIssue");

        It should_return_a_command_with_parameters_of_type_command_options = () => command.Parameters.ShouldBeOfType<CreateIssueCommandOptions>();

        It should_return_a_command_with_parameters_set_correctly = () =>
        {
            var options = command.Parameters as CreateIssueCommandOptions;
            options.Summary.ShouldEqual("\"Summary\"");
            options.Description.ShouldEqual("\"Description\"");
        };
        static CommandParser commandParser;
        static Command command;
    }
}