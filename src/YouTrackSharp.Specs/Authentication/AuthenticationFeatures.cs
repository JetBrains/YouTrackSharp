using Machine.Specifications;

namespace YouTrackSharp.Specs.Authentication
{
    [Subject("Authenticating")]
    public class when_provided_valid_username_and_password  
    {
        Establish context = () =>
        {
            youtrack = new YouTrackServer("youtrack.jetbrains.net");
        };

        Because of = () =>
        {
            youtrack.Login("youtrackapi", "youtrackapi");

        };

        It should_have_property_IsAuthenticated_set_to_true = () =>
        {
            youtrack.IsAuthenticated.ShouldBeTrue();
        };

        static YouTrackServer youtrack;
    }

    [Subject("Authenticating")]
    public class when_provided_invalid_username_and_password
    {
        Establish context = () =>
        {
            youtrack = new YouTrackServer("youtrack.jetbrains.net");
        };

        Because of = () =>
        {
            youtrack.Login("YouTrackSelfTestUser", "fdfdfd");

        };

        It should_have_property_IsAuthenticated_set_to_false = () =>
        {
            youtrack.IsAuthenticated.ShouldBeFalse();
        };

        static YouTrackServer youtrack;
    }

}