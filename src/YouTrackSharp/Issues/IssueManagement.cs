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
using Version = YouTrackSharp.Projects.Version;

namespace YouTrackSharp.Issues {
    public class IssueManagement {
        private static readonly List<string> PresetFields = new List<string> {
            "assignee",
            "priority",
            "type",
            "subsystem",
            "state",
            "fixVersions",
            "affectsVersions",
            "fixedInBuild",
            "summary",
            "description",
            "project",
            "permittedgroup"
        };

        private readonly IConnection theConnection;

        public IssueManagement(IConnection aConnection) {
            theConnection = aConnection;
        }

        /// <summary>
        /// Retrieve an issue by id
        /// </summary>
        /// <param name="issueId">Id of the issue to retrieve</param>
        /// <returns>An instance of Issue if successful or InvalidRequestException if issues is not found</returns>
        public Issue GetIssue(string issueId) {
            try {
                dynamic response = theConnection.Get<Issue>(String.Format("issue/{0}", issueId));

                if (response != null) {
                    response.Id = response.id;

                    foreach (var str in PresetFields)
                    {
                        if (!((Issue) response).GetDynamicMemberNames().Contains(str))
                        {
                            response[-1] = new KeyValuePair<string, object>(str, null);
                        }
                    }
                    return response;
                }
                return null;
            } catch (HttpException exception) {
                throw new InvalidRequestException(
                    String.Format(Language.YouTrackClient_GetIssue_Issue_not_found___0_, issueId), exception);
            }
        }

        public string CreateIssue(Issue issue) {
            if (!theConnection.IsAuthenticated)
                throw new InvalidRequestException(Language.YouTrackClient_CreateIssue_Not_Logged_In);

            try {
                var fieldList = issue.ToExpandoObject();

                var response = theConnection.Post("issue", fieldList, HttpContentTypes.ApplicationJson);

                var customFields = fieldList.Where(field => !PresetFields.Contains(field.Key.ToLower())).ToDictionary(field => field.Key, field => field.Value);

                foreach (var customField in customFields)
                    ApplyCommand(response.id, string.Format("{0} {1}", customField.Key, customField.Value), "Applying custom field");
                return response.id;
            } catch (HttpException httpException) {
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
        public IEnumerable<Issue> GetAllIssuesForProject(string projectIdentifier, int max = int.MaxValue, int start = 0) {
            return theConnection.Get<MultipleIssueWrapper, Issue>(string.Format("project/issues/{0}?max={1}&after={2}",
                                                                                projectIdentifier, max, start));
        }

        public IEnumerable<Issue> GetAllIssuesForVersion(string aVersionName) {
            return theConnection.Get<MultipleIssueWrapper, Issue>("issue?filter=version%3A+{" + HttpUtility.UrlEncode(aVersionName) + "}");
        }

        public IEnumerable<Issue> GetAllIssuesForVersion(Version aVersion) {
            return GetAllIssuesForVersion(aVersion.Name);
        }

        /// <summary>
        /// Retrieve comments for a particular issue
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        public IEnumerable<Comment> GetCommentsForIssue(string issueId) {
            return theConnection.Get<IEnumerable<Comment>>(String.Format("issue/comments/{0}", issueId));
        }

        public bool CheckIfIssueExists(string issueId) {
            try {
                theConnection.Head(string.Format("issue/{0}/exists", issueId));
                return theConnection.HttpStatusCode == HttpStatusCode.OK;
            } catch (HttpException httpException) {
                throw new InvalidRequestException(httpException.StatusDescription, httpException);
            }
        }

        public void AttachFileToIssue(string issuedId, string path) {
            theConnection.PostFile(string.Format("issue/{0}/attachment", issuedId), path);

            if (theConnection.HttpStatusCode != HttpStatusCode.Created)
                throw new InvalidRequestException(theConnection.HttpStatusCode.ToString());
        }

        public void ApplyCommand(string issueId, string command, string comment, bool disableNotifications = false, string runAs = "") {
            if (!theConnection.IsAuthenticated)
                throw new InvalidRequestException(Language.YouTrackClient_CreateIssue_Not_Logged_In);

            try {
                dynamic commandMessage = new ExpandoObject();

                commandMessage.command = command;
                commandMessage.comment = comment;
                if (disableNotifications)
                    commandMessage.disableNotifications = disableNotifications;
                if (!string.IsNullOrWhiteSpace(runAs))
                    commandMessage.runAs = runAs;

                theConnection.Post(string.Format("issue/{0}/execute", issueId), commandMessage);
            } catch (HttpException httpException) {
                throw new InvalidRequestException(httpException.StatusDescription, httpException);
            }
        }

        public IEnumerable<Issue> GetIssuesBySearch(string searchString, int max = int.MaxValue, int start = 0) {
            var encodedQuery = HttpUtility.UrlEncode(searchString);

            return
                theConnection.Get<MultipleIssueWrapper, Issue>(string.Format("project/issues?filter={0}&max={1}&after={2}",
                                                                             encodedQuery, max, start));
        }

        public int GetIssueCount(string searchString) {
            var encodedQuery = HttpUtility.UrlEncode(searchString);

            try {
                var count = -1;

                while (count < 0) {
                    var countObject = theConnection.Get<Count>(string.Format("issue/count?filter={0}", encodedQuery));

                    count = countObject.Entity.Value;
                    Thread.Sleep(3000);
                }

                return count;
            } catch (HttpException httpException) {
                throw new InvalidRequestException(httpException.StatusDescription, httpException);
            }
        }

        public void Delete(string id) {
            theConnection.Delete(string.Format("issue/{0}", id));
        }
    }
}
