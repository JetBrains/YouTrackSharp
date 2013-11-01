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
using System.Net;
using System.Security;
using System.Security.Authentication;
using Machine.Specifications;
using YouTrackSharp.Admin;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Specs.Helpers;

namespace YouTrackSharp.Specs.Specs
{
    [Subject(typeof (Connection))]
    public class when_authenticating_with_valid_username_and_password_given_valid_connection_details:  YouTrackConnection
    {

        Because of = () => connection.Authenticate("youtrackapi", "youtrackapi");

        It should_succeed = () => connection.IsAuthenticated.ShouldBeTrue();

    }

    [Subject(typeof(Connection))]
    public class when_authenticating_with_valid_netword_credentials_given_valid_connection_details : YouTrackConnection
    {
        Because of = () => connection.Authenticate(new NetworkCredential("youtrackapi", "youtrackapi"));

        It should_succeed = () => connection.IsAuthenticated.ShouldBeTrue();
    }

    [Subject(typeof(Connection))]
    public class when_authenticating_with_valid_username_and_secure_string_given_valid_connection_details : YouTrackConnection
    {
        Because of = () =>
        {
            var secure = new SecureString();
            foreach (var c in "youtrackapi")
                secure.AppendChar(c);
            connection.Authenticate(new NetworkCredential("youtrackapi", secure));
        };

        It should_succeed = () => connection.IsAuthenticated.ShouldBeTrue();
    }

    [Subject(typeof (Connection))]
    public class when_authenticating_with_invalid_username_and_or_password_given_valid_connection_details:YouTrackConnection
    {

        Because of = () => { exception = Catch.Exception( () => connection.Authenticate("YouTrackSelfTestUser", "fdfdfd")); };

        It should_throw_authentication_exception = () => exception.ShouldBeOfType<AuthenticationException>();
       

        static Exception exception;
    }

    [Subject(typeof (Connection))]
    public class when_requesting_current_logged_in_user_given_authenticated_details: AuthenticatedYouTrackConnection
    {
        Because of = () => { user = connection.GetCurrentAuthenticatedUser(); };

        It should_contain_valid_username = () => user.Username.ShouldEqual("youtrackapi");

        It should_contain_valid_fullname = () => user.FullName.ShouldEqual("YouTrack API");

        static User user;
    }
}