#region License

// Distributed under the BSD License
//   
// YouTrackSharp Copyright (c) 2010-2012, Hadi Hariri and Contributors
// All rights reserved.
//   
//  Redistribution and use in source and binary forms, with or without
//  modification, are permitted provided that the following conditions are met:
//      * Redistributions of source code must retain the above copyright
//         notice, this list of conditions and the following disclaimer.
//      * Redistributions in binary form must reproduce the above copyright
//         notice, this list of conditions and the following disclaimer in the
//         documentation and/or other materials provided with the distribution.
//      * Neither the name of Hadi Hariri nor the
//         names of its contributors may be used to endorse or promote products
//         derived from this software without specific prior written permission.
//   
//   THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS
//   "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED
//   TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A 
//   PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL 
//   <COPYRIGHTHOLDER> BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL,
//   SPECIAL,EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT
//   LIMITED  TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE,
//   DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND  ON ANY
//   THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
//   (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF
//   THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
//   

#endregion

using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using EasyHttp.Http;
using YouTrackSharp.Infrastructure;
using HttpException = EasyHttp.Infrastructure.HttpException;

namespace YouTrackSharp.Issues
{
    public class IssueManagement
    {
        static readonly List<string> PresetFields = new List<string>()
                                                     {
                                                         "assignee", "priority", "type", "subsystem", "state",
                                                         "fixVersions", "affectsVersions", "fixedInBuild", "summary",
                                                         "description", "project", "permittedgroup"
                                                     };
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

        /// <summary>
        /// Retrieve WorkItems of an issue by id
        /// </summary>
        /// <param name="issueId">Id of the issue to retrieve</param>
        /// <returns>An instance of WorkItems of an Issue if successful or InvalidRequestException if issues is not found</returns>
        public WorkItems GetWorkItems(string issueId)
        {
            try
            {
                return _connection.Get<WorkItems>(String.Format("issue/{0}/timetracking/workitem/", issueId));
            }
            catch (HttpException exception)
            {
                throw new InvalidRequestException(String.Format(Language.YouTrackClient_GetIssue_Issue_not_found___0_, issueId), exception);
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
                var fieldList = issue.ToExpandoObject();
                
                var response = _connection.Post("issue", fieldList, HttpContentTypes.ApplicationJson);

                var customFields = fieldList.Where(field => !PresetFields.Contains(field.Key.ToLower())).ToDictionary(field => field.Key, field => field.Value);

                foreach (var customField in customFields)
                {
                    ApplyCommand(response.id, string.Format("{0} {1}", customField.Key, customField.Value), string.Empty);
                }
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

        public void ApplyCommand(string issueId, string command, string comment, bool disableNotifications = false, string runAs = "")
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
                if (disableNotifications)
                    commandMessage.disableNotifications = disableNotifications;
                if (!string.IsNullOrWhiteSpace(runAs))
                    commandMessage.runAs = runAs;

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

            return
                _connection.Get<MultipleIssueWrapper, Issue>(string.Format("project/issues?filter={0}&max={1}&after={2}",
                                                                           encodedQuery, max, start));
        }

        public int GetIssueCount(string searchString)
        {
            var encodedQuery = HttpUtility.UrlEncode(searchString);

            try
            {
                var count = -1;

                while (count < 0)
                {
                    var countObject = _connection.Get<Count>(string.Format("issue/count?filter={0}", encodedQuery));

                    count = countObject.Entity.Value;
                    Thread.Sleep(3000);
                }

                return count;
            }
            catch (HttpException httpException)
            {
                throw new InvalidRequestException(httpException.StatusDescription, httpException);
            }
        }

        public void Delete(string id)
        {
            _connection.Delete(string.Format("issue/{0}", id));
        }
    }
}