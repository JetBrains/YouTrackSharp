#region License
// Distributed under the BSD License
//  
// YouTrackSharp Copyright (c) 2011-2011, Hadi Hariri and Contributors
// All rights reserved.
//  
// Redistribution and use in source and binary forms, with or without
// modification, are permitted provided that the following conditions are met:
//     * Redistributions of source code must retain the above copyright
//        notice, this list of conditions and the following disclaimer.
//     * Redistributions in binary form must reproduce the above copyright
//        notice, this list of conditions and the following disclaimer in the
//        documentation and/or other materials provided with the distribution.
//     * Neither the name of Hadi Hariri nor the
//        names of its contributors may be used to endorse or promote products
//        derived from this software without specific prior written permission.
//  
//  THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//  "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
//  TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
//  PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
//  <COPYRIGHTHOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//  SPECIAL,EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//  LIMITED  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//  DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  ON ANY
//  THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//  (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//  THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//  
#endregion

using Machine.Specifications;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Issues;

namespace YouTrackSharp.Specs.Bugs
{
    public class YTSRP8
    {
        Establish context = () =>
        {
            connection = new Connection("youtrack.jetbrains.net");
            connection.Authenticate("youtrackapi", "youtrackapi");

        };

        Because of = () =>
        {
            var issueManagement = new IssueManagement(connection);
            dynamic issue1 = new Issue();
            
            issue1.Summary = "authbug1";
            issue1.Description = "description1";
            issue1.ProjectShortName = "SB";

            dynamic issue2 = new Issue();
            
            issue2.Summary = "authbug2";
            issue2.Description = "description2";
            issue2.ProjectShortName = "SB";
            
            issueId1 = issueManagement.CreateIssue(issue1);
            issueId2 = issueManagement.CreateIssue(issue2);

        };

        It should_create_issue1 = () => issueId1.ShouldNotBeNull();
        It should_create_issue2 = () => issueId2.ShouldNotBeNull();

        static Connection connection;
        static string issueId1;
        static string issueId2;
    }
}