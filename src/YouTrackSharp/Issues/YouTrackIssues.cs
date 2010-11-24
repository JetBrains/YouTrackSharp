using System;
using System.Collections.Generic;
using System.IO;
using System.Web;
using YouTrackSharp.DataModel;
using YouTrackSharp.Infrastructure;
using HttpException = EasyHttp.Http.HttpException;

namespace YouTrackSharp.Issues
{
    public class YouTrackIssues
    {
        private readonly YouTrackServer _youTrackServer;
        private readonly IJsonIssueConverter _jsonIssueConverter;

        public YouTrackIssues(YouTrackServer youTrackServer)
        {
            _youTrackServer = youTrackServer;
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
                dynamic response = _youTrackServer.Get("issue/{0}", issueId);

                return _jsonIssueConverter.ConvertFromDynamicFields(response);
            }
            catch (HttpException exception)
            {
                throw new InvalidRequestException(String.Format(Language.YouTrackClient_GetIssue_Issue_not_found___0_, issueId), exception);
            }
        }

        public string CreateIssue(NewIssueMessage issueMessage)
        {
            if (!_youTrackServer.IsAuthenticated)
            {
                throw new InvalidRequestException(Language.YouTrackClient_CreateIssue_Not_Logged_In);
            }

            try
            {
                dynamic response = _youTrackServer.Post("issue", issueMessage);


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
            dynamic response = _youTrackServer.Get("project/issues/{0}?max={1}&after={2}", projectIdentifier, max, start);

            dynamic issues = response.issue;

            List<Issue> list = new List<Issue>();

            foreach (dynamic entry in issues)
            {
                dynamic issue = _jsonIssueConverter.ConvertFromDynamic(entry);

                list.Add(issue);
            }

            return list;
        }

        /// <summary>
        /// Executes command on designated isse
        /// </summary>
        /// <param name="issueKey">Issue key</param>
        /// <param name="command">Command to execute</param>
        /// <param name="comment">Additional comment</param>
        /// <param name="commentVisibleGroup">Comment visibility group</param>
        /// <param name="disableNotifications">If true, prevents YouTrack from sending notifications for this command</param>
        public void ExecuteAction(string issueKey, string command, string comment, string commentVisibleGroup, bool disableNotifications)
        {
            string commentParam = comment != null ? "&comment=" + HttpUtility.UrlEncode(comment) : String.Empty;
            string commentVisibleGroupParam = commentVisibleGroup != null ? "&group=" + HttpUtility.UrlEncode(commentVisibleGroup) : String.Empty;
            string commandParam = command != null ? "command=" + HttpUtility.UrlEncode(command) : String.Empty;
            string disableNotificationParam = disableNotifications ? "&disableNotifications=true" : String.Empty;
            string actionUri = (String.Format("issue/execute/{0}?{1}{2}{3}{4}", issueKey, commandParam, commentParam, commentVisibleGroupParam, disableNotificationParam));
            _youTrackServer.HttpPost(actionUri, String.Format("Execute action '{0}' for issue {1}", command, issueKey));
        }


        /// <summary>
        /// Retrieves a list of comments for a designated issue
        /// </summary>
        /// <param name="issueKey">Issue key</param>
        public IEnumerable<YouTrackComment> GetComments(string issueKey)
        {
            string getCommentsUri = string.Format("issue/comments/{0}", issueKey);
            return XmlConverter.ExtractComments(_youTrackServer.HttpGet(getCommentsUri, "GetComments"));
        }

        /// <summary>
        /// Checks if issue with given id exists
        /// </summary>
        /// <param name="issueKey">Issue id</param>
        /// <returns>True if issue exists</returns>
        public bool IssueExists(string issueKey)
        {
            string existsUri = String.Format("issue/{0}/exists", issueKey);
            try
            {
                _youTrackServer.HttpGet(existsUri, "IssueExists");
            }
            catch
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Filters the issues using a query
        /// </summary>
        /// <param name="projectKey">Project to search the issues in. If null, search is performed over all issues</param>
        /// <param name="query">Query</param>
        /// <param name="after">Starting point in the result</param>
        /// <param name="max">Maximal number of returned items</param>
        /// <returns>List of issues that match the query</returns>
        public IEnumerable<Issue> FilterIssues(string projectKey, string query, int? after, int? max)
        {
            string afterParameter = after != null ? "&after=" + after.Value : String.Empty;
            string maxParameter = max != null ? "&max=" + max.Value : String.Empty;
            string filterUri = String.Format("project/issues{0}?filter={1}{2}{3}", projectKey != null ? "/" + projectKey : String.Empty, HttpUtility.UrlEncode(query), afterParameter, maxParameter);
            return XmlConverter.ExtractIssues(_youTrackServer.HttpGet(filterUri, "FilterIssues"));
        }
    }
}