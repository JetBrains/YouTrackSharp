using System.Collections.Generic;

namespace YouTrackSharp.Issues
{
    public interface IIssueManagement
    {
        /// <summary>
        /// Retrieve an issue by id
        /// </summary>
        /// <param name="issueId">Id of the issue to retrieve</param>
        /// <returns>An instance of Issue if successful or InvalidRequestException if issues is not found</returns>
        Issue GetIssue(string issueId);

        string CreateIssue(Issue issue);

        /// <summary>
        /// Retrieves a list of issues 
        /// </summary>
        /// <param name="projectIdentifier">Project Identifier</param>
        /// <param name="max">[Optional] Maximum number of issues to return. Default is int.MaxValue</param>
        /// <param name="start">[Optional] The number by which to start the issues. Default is 0. Used for paging.</param>
        /// <returns>List of Issues</returns>
        IEnumerable<Issue> GetAllIssuesForProject(string projectIdentifier, int max = int.MaxValue, int start = 0);

        /// <summary>
        /// Retrieve comments for a particular issue
        /// </summary>
        /// <param name="issueId"></param>
        /// <returns></returns>
        IEnumerable<Comment> GetCommentsForIssue(string issueId);

        bool CheckIfIssueExists(string issueId);

        void AttachFileToIssue(string issuedId, string path);

        void ApplyCommand(string issueId, string command, string comment, bool disableNotifications = false, string runAs = "");

        void UpdateIssue(string issueId, string summary, string description);

        IEnumerable<Issue> GetIssuesBySearch(string searchString, int max = int.MaxValue, int start = 0);

        int GetIssueCount(string searchString);

        void Delete(string id);

        void DeleteComment(string issueId, string commentId, bool deletePermanently);
    }
}