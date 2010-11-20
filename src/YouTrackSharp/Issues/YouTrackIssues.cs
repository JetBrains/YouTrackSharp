using System;
using System.Collections.Generic;
using EasyHttp.Http;
using YouTrackSharp.Infrastructure;

namespace YouTrackSharp.Issues
{
    public class YouTrackIssues
    {
        readonly YouTrackConnection _youTrackConnection;
        readonly IJsonIssueConverter _jsonIssueConverter;

        public YouTrackIssues(YouTrackConnection youTrackConnection)
        {
            _youTrackConnection = youTrackConnection;
            _jsonIssueConverter = new JsonIssueConverter();
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
                
                var response = _youTrackConnection.Get("issue/{0}", issueId);

                return _jsonIssueConverter.ConvertFromDynamicFields(response);
            }
            catch (HttpException exception)
            {
                throw new InvalidRequestException(String.Format(Language.YouTrackClient_GetIssue_Issue_not_found___0_, issueId), exception);
            }
        }

        public string CreateIssue(NewIssueMessage issueMessage)
        {
            if (!_youTrackConnection.IsAuthenticated)
            {
                throw new InvalidRequestException(Language.YouTrackClient_CreateIssue_Not_Logged_In);
            }

            try
            {
                var response = _youTrackConnection.Post("issue", issueMessage);

             
                return response.issue.id;

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
        public IList<Issue> GetIssues(string projectIdentifier, int max = int.MaxValue, int start = 0)
        {
            dynamic response = _youTrackConnection.Get("project/issues/{0}?max={1}&after={2}", projectIdentifier, max, start);

            dynamic issues = response.issue;

            var list = new List<Issue>();

            foreach (dynamic entry in issues)
            {
                var issue = _jsonIssueConverter.ConvertFromDynamic(entry);

                list.Add(issue);
            }

            return list;
        }


    }
}