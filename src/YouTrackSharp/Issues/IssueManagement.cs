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
using System.ComponentModel;
using System.Dynamic;
using System.IO;
using System.Net;
using System.Threading;
using System.Web;
using System.Xml;
using System.Xml.Linq;
using EasyHttp.Codecs.JsonFXExtensions;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using JsonFx.Serialization;
using YouTrackSharp.Infrastructure;
using HttpException = EasyHttp.Infrastructure.HttpException;

namespace YouTrackSharp.Issues
{
    public class IssueManagement
    {
        readonly IConnection _connection;

        public IssueManagement(IConnection connection)
        {
            _connection = connection;
        }

        /// <summary>
        /// Retrieve an issue by id
        /// </summary>
        /// <param name="issueId">Id of the issue to retrieve</param>
        /// <returns>An instance of Issue if successful or InvalidRequestException if issues is not found</returns>
        public Issue GetIssue(string issueId)
        {
            try
            {
                dynamic response = _connection.Get<Issue>(String.Format("issue/{0}", issueId));

                if (response != null)
                {
                    response.Id = response.id;

                    return response;
                }
                return null;
            }
            catch (HttpException exception)
            {
                throw new InvalidRequestException(
                    String.Format(Language.YouTrackClient_GetIssue_Issue_not_found___0_, issueId), exception);
            }
        }

        public string CreateIssue(Issue issue)
        {
            if (!_connection.IsAuthenticated)
            {
                throw new InvalidRequestException(Language.YouTrackClient_CreateIssue_Not_Logged_In);
            }

            try
            {
 
                var response = _connection.Post("issue", issue.ToExpandoObject(), HttpContentTypes.ApplicationJson);

       
                return response.id;
            }
            catch (HttpException httpException)
            {
                throw new InvalidRequestException(httpException.StatusDescription, httpException);
            }
        }

 
        /// <summary>
        /// Retrieves a list of issues 
        /// </summary>
        /// <param name="projectIdentifier">Project Identifier</param>
        /// <param name="max">[Optional] Maximum number of issues to return. Default is int.MaxValue</param>
        /// <param name="start">[Optional] The number by which to start the issues. Default is 0. Used for paging.</param>
        /// <returns>List of Issues</returns>
        public IEnumerable<Issue> GetAllIssuesForProject(string projectIdentifier, int max = int.MaxValue, int start = 0)
        {
                return
                    _connection.Get<MultipleIssueWrapper, Issue>(string.Format("project/issues/{0}?max={1}&after={2}",
                                                                               projectIdentifier, max, start));
                
        }

        /// <summary>
        /// Retrieve comments for a particular issue
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetCommentsForIssue(string issueId)
        {
            return _connection.Get<IEnumerable<Comment>>(String.Format("issue/comments/{0}", issueId));
        }

        public bool CheckIfIssueExists(string issueId)
        {
            try
            {
                _connection.Head(string.Format("issue/{0}/exists", issueId));
                return _connection.HttpStatusCode == HttpStatusCode.OK;
            }
            catch (HttpException httpException)
            {
                throw new InvalidRequestException(httpException.StatusDescription, httpException);
            }
        }

        public void AttachFileToIssue(string issuedId, string path)
        {
            _connection.PostFile(string.Format("issue/{0}/attachment", issuedId), path);
            
            if (_connection.HttpStatusCode != HttpStatusCode.Created)
            {
                throw new InvalidRequestException(_connection.HttpStatusCode.ToString());
            }
        }

        public void ApplyCommand(string issueId, string command, string comment)
        {
            if (!_connection.IsAuthenticated)
            {
                throw new InvalidRequestException(Language.YouTrackClient_CreateIssue_Not_Logged_In);
            }

            try
            {
                dynamic commandMessage = new ExpandoObject();


                commandMessage.command = command;
                commandMessage.comment = comment;
                
                _connection.Post(string.Format("issue/{0}/execute", issueId), commandMessage);

            }
            catch (HttpException httpException)
            {
                throw new InvalidRequestException(httpException.StatusDescription, httpException);
            }
                
        }

        public IEnumerable<Issue> GetIssuesBySearch(string searchString, int max = int.MaxValue, int start = 0)
        {
          
            var encodedQuery = HttpUtility.UrlEncode(searchString);

            try
            {
                return
                    _connection.Get<MultipleIssueWrapper, Issue>(string.Format("project/issues?filter={0}&max={1}&after={2}",
                                                                               encodedQuery, max, start));

            }
            catch (DeserializationException)
            {
                // TODO: BIG CRAPPY UGLY HACK THAT IS HERE UNTIL YOUTRACK SERVER IS SOLVED. THIS WOULD ACTUALLY
                // APPLY TO ALL ISSUES. SEE http://youtrack.codebetter.com/issue/YTSRP-9
                var issues = _connection.Get<SingleIssueWrapperTemporaryHackUntilYouTrackServerIsFixed>(string.Format("project/issues?filter={0}&max={1}&after={2}",
                                                                               encodedQuery, max, start));

              

                return new List<Issue>() { issues.issue };
            }                
                
        }

        public int GetIssueCount(string searchString)
        {
            var encodedQuery = HttpUtility.UrlEncode(searchString);

            try
            {
                var count = -1;

                while (count < 0) {

                    count = _connection.Get<int>(string.Format("issue/count?filter={0}", encodedQuery));
                    Thread.Sleep(3000);
                }
                
                return count;

            }
            catch (HttpException httpException)
            {
                throw new InvalidRequestException(httpException.StatusDescription, httpException);
            }
        } 
    }
}