﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using EasyHttp.Http;
using EasyHttp.Infrastructure;
using YouTrackSharp.Infrastructure;
using YouTrackSharp.Server;


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
               var response = _connection.Get<SingleIssueWrapper>("issue/{0}", issueId);

                var issue = TypeDescriptor.GetConverter(typeof (Issue)).ConvertFrom(response.field) as Issue;

                issue.Id = response.id;

                return issue;
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
                dynamic newIssueMessage = new ExpandoObject();

                newIssueMessage.project = issue.ProjectShortName;
                newIssueMessage.description = issue.Description;
                newIssueMessage.summary = issue.Summary;
                newIssueMessage.assignee = issue.Assignee;

                var response = _connection.Post<dynamic>("issue", newIssueMessage, HttpContentTypes.ApplicationJson);

             
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
        public IEnumerable<Issue> GetIssues(string projectIdentifier, int max = int.MaxValue, int start = 0)
        {
            return _connection.Get<MultipleIssueWrapper, Issue>(string.Format("project/issues/{0}?max={1}&after={2}", projectIdentifier, max, start));
        }

        /// <summary>
        /// Retrieve comments for a particular issue
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetCommentsForIssue(string issueId)
        {

            try
            {
                var response = _connection.Get<MultipleCommentWrapper>("issue/comments/{0}", issueId);

                return response.comment;

            }
            catch (HttpException httpException)
            {
                throw new InvalidRequestException(httpException.StatusDescription, httpException);   
            }

        }
    }
}