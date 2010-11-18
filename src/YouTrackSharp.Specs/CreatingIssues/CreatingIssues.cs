using Machine.Specifications;

namespace YouTrackSharp.Specs.CreatingIssues
{
    [Subject("Creating Issues")]
    public class when_creating_a_new_issue_with_valid_information
    {
        Establish context = () =>
        {
            _youTrackClient = new YouTrackClient("youtrack.jetbrains.net");
        };

        Because of = () =>
        {
            var issue = new Issue();

            //response = youTrack.CreateIssue(issue);
        };

        It should_action = () =>
        {

        };

        static YouTrackClient _youTrackClient;
    }
}