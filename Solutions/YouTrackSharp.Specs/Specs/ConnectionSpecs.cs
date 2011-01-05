#region Settings
#pragma warning disable 169
// ReSharper disable InconsistentNaming
// ReSharper disable UnusedMember.Global
// ReSharper disable UnusedMember.Local
#endregion

namespace YouTrackSharp.Specs.Specs
{
    #region Using Directives

    using Machine.Specifications;

    using YouTrackSharp.Infrastructure;
    using YouTrackSharp.Server;

    #endregion

    [Subject("Connection")]
    public class when_provided_valid_username_and_password
    {
        Establish context = () =>
        {
            connection = new Connection("youtrack.jetbrains.net");
        };

        Because of = () =>
        {
            connection.Authenticate("youtrackapi", "youtrackapi");

        };

        It should_have_property_IsAuthenticated_set_to_true = () =>
        {
            connection.IsAuthenticated.ShouldBeTrue();
        };

        static Connection connection;
    }

    [Subject("Connection")]
    public class when_provided_invalid_username_and_password
    {
        Establish context = () =>
        {
            connection = new Connection("youtrack.jetbrains.net");

        };

        Because of = () =>
        {
            connection.Authenticate("YouTrackSelfTestUser", "fdfdfd");

        };

        It should_have_property_IsAuthenticated_set_to_false = () =>
        {
            connection.IsAuthenticated.ShouldBeFalse();
        };

        static Connection connection;
    }

    [Subject("Connection")]
    public class when_requesting_current_logged_user
    {
        Establish context = () =>
        {
            connection = new Connection("youtrack.jetbrains.net");


            connection.Authenticate("youtrackapi","youtrackapi");
        };

        Because of = () =>
        {

            user = connection.GetCurrentAuthenticatedUser();

        };

        It should_return_user_information = () =>
        {
            user.Username.ShouldEqual("youtrackapi");
            user.FullName.ShouldEqual("YouTrack API");
        };

        static IConnection connection;
        static User user;
    }
}