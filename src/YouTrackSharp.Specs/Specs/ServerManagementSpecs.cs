using System;
using Machine.Specifications;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Server;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{
    [Subject("Server")]
    public class when_requesting_user_information_of_existing_user_with_valid_authorization: AuthenticatedYouTrackConnectionSetup
    {
        Establish context = () =>
        {
            server = new ServerManagement(connection);
        };

        Because of = () =>
        {
            user = server.GetUserByUserName("youtrackapi");
        };

        It should_return_user_information = () =>
        {
            user.ShouldNotBeNull();
            user.Username.ShouldEqual("youtrackapi");
            user.FullName.ShouldEqual("YouTrack API");
        };

        static User user;
        static ServerManagement server;
    }

    [Subject("Server")]
    public class when_requesting_user_information_of_non_existing_user_with_invalid_authorization: AuthenticatedYouTrackConnectionSetup
    {
        Establish context = () =>
        {
            server = new ServerManagement(connection);
        };
        
        Because of = () =>
        {
            exception = Catch.Exception(() => server.GetUserByUserName("jdklgjdfklgjfld"));
        };

        It should_throw_invalid_authorization_exception_with_message_insufficient_rights = () =>
        {
            exception.ShouldBeOfType<InvalidRequestException>();
            exception.Message.ShouldEqual("Insufficient rights");
        };

        static Exception exception;
        static ServerManagement server;
    }

    [Subject("Server")]
    public class when_requesting_user_information_of_existing_user_with_invalid_authorization: YouTrackConnectionSetup
    {
        Establish context = () =>
        {
            server = new ServerManagement(connection);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => server.GetUserByUserName("root"));
        };

        It should_throw_invalid_authorization_exception_with_message_insufficient_rights = () =>
        {
            exception.ShouldBeOfType<InvalidRequestException>();
            exception.Message.ShouldEqual("Insufficient rights");
        };

        static User user;
        static Exception exception;
        static ServerManagement server;
    }

}