using System;
using System.Collections.Generic;
using System.Linq;
using Machine.Specifications;
using YouTrackSharp.Admin;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Server;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{
    [Subject("User Management")]
    public class when_requesting_user_information_of_existing_user_with_valid_authorization: AuthenticatedYouTrackConnectionSetup
    {
        Establish context = () =>
        {
            userManagement = new UserManagement(connection);
        };

        Because of = () =>
        {
            user = userManagement.GetUserByUserName("youtrackapi");
        };

        It should_return_user_information = () =>
        {
            user.ShouldNotBeNull();
            user.Username.ShouldEqual("youtrackapi");
            user.FullName.ShouldEqual("YouTrack API");
        };

        static User user;
        static UserManagement userManagement;
    }

    [Subject("User Management")]
    public class when_requesting_user_information_of_non_existing_user_with_invalid_authorization: AuthenticatedYouTrackConnectionSetup
    {
        Establish context = () =>
        {
            userManagement = new UserManagement(connection);
        };
        
        Because of = () =>
        {
            exception = Catch.Exception(() => userManagement.GetUserByUserName("jdklgjdfklgjfld"));
        };

        It should_throw_invalid_authorization_exception_with_message_insufficient_rights = () =>
        {
            exception.ShouldBeOfType<InvalidRequestException>();
            exception.Message.ShouldEqual("Insufficient rights");
        };

        static Exception exception;
        static UserManagement userManagement;
    }

    [Subject("User Management")]
    public class when_requesting_user_information_of_existing_user_with_invalid_authorization: YouTrackConnectionSetup
    {
        Establish context = () =>
        {
            userManagement = new UserManagement(connection);
        };

        Because of = () =>
        {
            exception = Catch.Exception(() => userManagement.GetUserByUserName("root"));
        };

        It should_throw_invalid_authorization_exception_with_message_insufficient_rights = () =>
        {
            exception.ShouldBeOfType<InvalidRequestException>();
            exception.Message.ShouldEqual("Insufficient rights");
        };

        static Exception exception;
        static UserManagement userManagement;
    }

    [Subject("User Management")]
    public class when_requesting_saved_filters_for_a_specific_user : AuthenticatedYouTrackConnectionSetup
    {
        Establish context = () =>
        {
            userManagement = new UserManagement(connection);
        };

        Because of = () =>
        {
            filters = userManagement.GetFiltersByUsername("youtrackapi");
        };

        It should_return_a_list_of_filters_for_the_specified_user = () =>
        {
            filters.Count().ShouldBeGreaterThan(0);
        };

        It should_contain_valid_query_data = () =>
        {
            filters.First().Name.ShouldNotBeEmpty();
            filters.First().Query.ShouldNotBeEmpty();
        };
        static UserManagement userManagement;
        static IEnumerable<Filter> filters;
    }
}