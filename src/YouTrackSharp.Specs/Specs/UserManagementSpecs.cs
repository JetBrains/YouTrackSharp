#region License
// Distributed under the BSD License
// =================================
// 
// Copyright (c) 2010-2011, Hadi Hariri
// All rights reserved.
// 
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//       notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//       notice, this list of conditions and the following disclaimer in the
//       documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//       names of its contributors may be used to endorse or promote products
//       derived from this software without specific prior written permission.
// 
// THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
// ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
// WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
// DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> BE LIABLE FOR ANY
// DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
// (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
// LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
// ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
// (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
// SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
// =============================================================
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using EasyHttp.Infrastructure;
using Machine.Specifications;
using YouTrackSharp.Admin;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{
    [Subject(typeof (UserManagement))]
    public class when_requesting_user_information_of_existing_user_given_authenticated_connection_and_existing_users : AuthenticatedYouTrackConnection
    {
        Establish context = () => { userManagement = new UserManagement(connection); };

        Because of = () => { user = userManagement.GetUserByUserName("youtrackapi"); };

        It should_return_user_information = () => user.ShouldNotBeNull();

        It should_contain_valid_username = () => user.Username.ShouldEqual("youtrackapi");

        It should_contain_valid_fullname = () => user.FullName.ShouldEqual("YouTrack API");

        static User user;
        static UserManagement userManagement;
    }

    [Subject(typeof (UserManagement))]
    public class when_requesting_user_information_of_non_existing_user_given_authenticated_connection_and_existing_users : AuthenticatedYouTrackConnection
    {
        Establish context = () => { userManagement = new UserManagement(connection); };

        Because of = () => { exception = Catch.Exception(() => userManagement.GetUserByUserName("jdklgjdfklgjfld")); };

        It should_throw_invalid_authorization_exception = () => exception.ShouldBeOfType<InvalidRequestException>();

        It should_contain_message_insufficient_rights = () => exception.Message.ShouldEqual("Insufficient rights");


        static Exception exception;
        static UserManagement userManagement;
    }

    [Subject(typeof (UserManagement))]
    public class when_requesting_user_information_of_a_user_given_authenticated_connection_and_existing_users : YouTrackConnection
    {
        Establish context = () => { userManagement = new UserManagement(connection); };

        Because of = () => { exception = Catch.Exception(() => userManagement.GetUserByUserName("root")); };

        It should_throw_invalid_authorization_exception = () => exception.ShouldBeOfType<HttpException>();

        It should_contain_message_insufficient_rights = () => exception.Message.ShouldEqual("Unauthorized Unauthorized");

        static Exception exception;
        static UserManagement userManagement;
    }

    [Subject(typeof (UserManagement))]
    public class when_requesting_saved_filters_for_a_specific_user_given_authenticated_connection : AuthenticatedYouTrackConnection
    {
        Establish context = () => { userManagement = new UserManagement(connection); };

        Because of = () => { filters = userManagement.GetFiltersByUsername("youtrackapi"); };

        It should_return_a_list_of_filters_for_the_specified_user = () => filters.ShouldNotBeEmpty();

        It should_contain_valid_name = () => filters.First().Name.ShouldNotBeEmpty();

        It should_contain_valid_query = () => filters.First().Query.ShouldNotBeEmpty();

        static UserManagement userManagement;
        static IEnumerable<Filter> filters;
    }

		[Subject(typeof(UserManagement))]
        [Ignore("The server says insuficient rights.")]
        public class when_requesting_all_users_given_authenticated_connection_and_existing_users : AuthenticatedYouTrackConnection
		{
			Establish context = () => { userManagement = new UserManagement(connection); };

			Because of = () => { users = userManagement.GetAllUsers(); };

			It should_return_all_users = () => users.ShouldNotBeNull();
			It should_return_at_least_one_user = () => users.ShouldNotBeEmpty();

			static IEnumerable<User> users;
			static UserManagement userManagement;
		}
}