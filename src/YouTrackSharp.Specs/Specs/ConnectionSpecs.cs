using Machine.Specifications;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Server;

namespace YouTrackSharp.Specs.Specs
{
    [Subject(typeof (Connection), "given valid connection details")]
    public class when_authenticating_with_valid_username_and_password
    {
        Establish context = () => { connection = new Connection("youtrack.jetbrains.net"); };

        Because of = () => connection.Authenticate("youtrackapi", "youtrackapi");

        It should_succeed = () => connection.IsAuthenticated.ShouldBeTrue();

        static Connection connection;
    }

    [Subject(typeof (Connection), "given valid connection details")]
    public class when_authenticating_with_invalid_username_and_or_password
    {
        Establish context = () => { connection = new Connection("youtrack.jetbrains.net"); };

        Because of = () => { connection.Authenticate("YouTrackSelfTestUser", "fdfdfd"); };

        It should_not_succeed = () => connection.IsAuthenticated.ShouldBeFalse();

        static Connection connection;
    }

    [Subject(typeof (Connection), "given authenticated connection")]
    public class when_requesting_current_logged_in_user
    {
        Establish context = () =>
        {
            connection = new Connection("youtrack.jetbrains.net");


            connection.Authenticate("youtrackapi", "youtrackapi");
        };

        Because of = () => { user = connection.GetCurrentAuthenticatedUser(); };

        It should_contain_valid_username = () => user.Username.ShouldEqual("youtrackapi");

        It should_contain_valid_fullname = () => user.FullName.ShouldEqual("YouTrack API");

        static IConnection connection;
        static User user;
    }
}