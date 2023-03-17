using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace YouTrackSharp.Issues
{
    public interface IIssuesService
    {
        /// <summary>
        /// Get a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-issues.html#get-Issue-method">Get an Issue</a>.</remarks>
        /// <param name="issueId">Id of an issue to get.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then issue description in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <param name="wikifyContents">If set to <value>true</value>, then comments and issue text fields in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>/// <returns>The <see cref="Issue" /> that matches the requested <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<Issue> GetIssue(string issueId, bool wikifyDescription = false, bool wikifyContents = false);

        /// <summary>
        /// Checks whether an issue exists on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-issues.html#get-Issue-method">Check that an Issue Exists</a>.</remarks>
        /// <param name="issueId">Id of an issue to check.</param>
        /// <returns><value>True</value> if the issue exists, otherwise <value>false</value>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<bool> Exists(string issueId);

        /// <summary>
        /// Creates an issue on the server in a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues.html#create-Issue-method">Create New Issue</a>.</remarks>
        /// <param name="projectId">Id of the project to create an issue in.</param>
        /// <param name="issue">The <see cref="Issue" /> to create. At the minimum needs the Summary field populated.</param>
        /// <returns>The newly created <see cref="Issue" />'s id on the server.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<string> CreateIssue(string projectId, Issue issue);

        /// <summary>
        /// Updates an issue on the server in a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-issues.html#update-Issue-method">Update an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to update.</param>
        /// <param name="summary">Updated summary of the issue.</param>
        /// <param name="description">Updated description of the issue.</param>
        /// <param name="isMarkdown">Is the updated description of the issue in Markdown format? Setting the format to Markdown is supported in YouTrack versions 2018.2 and later.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task UpdateIssue(string issueId, string summary = null, string description = null, bool? isMarkdown = null);

        /// <summary>
        /// Applies a command to a specific issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-commands.html#create-CommandList-method">Apply Command to an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to apply the command to.</param>
        /// <param name="command">The command to apply. A command might contain a string of attributes and their values - you can change multiple fields with one complex command.</param>
        /// <param name="comment">A comment to add to an issue.</param>
        /// <param name="disableNotifications">When <value>true</value>, no notifications about changes made with the specified command will be sent. Defaults to <value>false</value>.</param>
        /// <param name="runAs">Login name for a user on whose behalf the command should be applied.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="command"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task ApplyCommand(string issueId, string command, string comment = null, bool disableNotifications = false, string runAs = null);

        /// <summary>
        /// Deletes an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-issues.html#delete-Issue-method">Delete an Issue</a>.</remarks>
        /// <param name="issueId">Id of an issue to delete.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task DeleteIssue(string issueId);

        /// <summary>
        /// Get links for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues-issueID-links.html#get_all-IssueLink-method">Get Links of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get links for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Link" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<IEnumerable<Link>> GetLinksForIssue(string issueId);

        /// <summary>
        /// Get issues in a project from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues.html#get_all-Issue-method">Find Issues belonging to a Project</a>.</remarks>
        /// <param name="projectId">Id of a project to get issues from.</param>
        /// <param name="filter">Apply a filter to issues in a project.</param>
        /// <param name="skip">The number of issues to skip before getting a list of issues.</param>
        /// <param name="take">Maximum number of issues to be returned. Defaults to the server-side default of the YouTrack server instance..</param>
        /// <param name="updatedAfter">Only issues updated after the specified date will be retrieved.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then issue description in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <param name="wikifyContents">If set to <value>true</value>, then comments and issue text fields in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<ICollection<Issue>> GetIssuesInProject(string projectId, string filter = null, int? skip = null,
            int? take = null, DateTime? updatedAfter = null, bool wikifyDescription = false, bool wikifyContents = false);

        /// <summary>
        /// Get issues from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues.html#get_all-Issue-method">Get the List of Issues</a>.</remarks>
        /// <param name="filter">Apply a filter to issues.</param>
        /// <param name="skip">The number of issues to skip before getting a list of issues.</param>
        /// <param name="take">Maximum number of issues to be returned. Defaults to the server-side default of the YouTrack server instance.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then issue description in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <param name="wikifyContents">If set to <value>true</value>, then comments and issue text fields in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>/// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<ICollection<Issue>> GetIssues(string filter = null, int? skip = null, int? take = null,
            bool wikifyDescription = false, bool wikifyContents = false);

        /// <summary>
        /// Get issue count from the server. This operation may be retried internally and take a while to complete.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issuesGetter-count.html">Get a Number of Issues</a>.</remarks>
        /// <param name="filter">Apply a filter to issues.</param>
        /// <returns>The number of <see cref="Issue" /> that match the specified filter.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<long> GetIssueCount(string filter = null);

        /// <summary>
        /// Get change history for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues-issueID-activities.html#get_all-ActivityItem-method">Get Changes of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get change history for.</param>
        /// <param name="vcsHistory">If set to <value>true</value>, vcs historical items will be additionally fetched.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Change" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<IEnumerable<Change>> GetChangeHistoryForIssue(string issueId, bool vcsHistory = false);
        
        /// <summary>
        /// Get comments for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues-issueID-comments.html#get_all-IssueComment-method">Get Comments of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get comments for.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then comments in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Comment" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<IEnumerable<Comment>> GetCommentsForIssue(string issueId, bool wikifyDescription = false);

        /// <summary>
        /// Adds a comment for an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues-issueID-comments.html#create-IssueComment-method">Create a Comment</a>.</remarks>
        /// <param name="issueId">Id of the issue to add comment to.</param>
        /// <param name="text">The text of the new comment.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="text"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task AddCommentForIssue(string issueId, string text);

        /// <summary>
        /// Updates a comment for an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-issues-issueID-comments.html#update-IssueComment-method">Update a Comment</a>.</remarks>
        /// <param name="issueId">Id of the issue to which the comment belongs.</param>
        /// <param name="commentId">Id of the comment to update.</param>
        /// <param name="text">The new text of the comment.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/>, <paramref name="commentId"/> or <paramref name="text"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task UpdateCommentForIssue(string issueId, string commentId, string text);

        /// <summary>
        /// Deletes a comment for an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-issues-issueID-comments.html#delete-IssueComment-method">Remove a Comment for an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to which the comment belongs.</param>
        /// <param name="commentId">Id of the comment to delete.</param>
        /// <param name="permanent">When <value>true</value>, the specified comment will be deleted permanently and can not be restored afterwards. Defaults to <value>false</value>.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="commentId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task DeleteCommentForIssue(string issueId, string commentId, bool permanent = false);

        /// <summary>
        /// Attaches a file to an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues-issueID-attachments.html#create-IssueAttachment-method">Attach File to an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to attach the file to.</param>
        /// <param name="attachmentName">Filename for the attachment.</param>
        /// <param name="attachmentStream">The <see cref="T:System.IO.Stream"/> to attach.</param>
        /// <param name="group">Attachment visibility group.</param>
        /// <param name="author">Creator of the attachment. Note to define author the 'Low-Level Administration' permission is required.</param>
        /// <param name="attachmentContentType">Content type of the attachment, for example text/plain or image/png.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/>, <paramref name="attachmentName"/> or <paramref name="attachmentStream"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task AttachFileToIssue(string issueId, string attachmentName, Stream attachmentStream, string group = null, string author = null, string attachmentContentType = null);

        /// <summary>
        /// Get attachments for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues-issueID-attachments.html#get_all-IssueAttachment-method">Get Attachments of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get comments for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Attachment" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<IEnumerable<Attachment>> GetAttachmentsForIssue(string issueId);

        /// <summary>
        /// Downloads an attachment from the server.
        /// </summary>
        /// <param name="attachmentUrl">The <see cref="T:System.Uri" /> of the attachment.</param>
        /// <returns>A <see cref="T:System.IO.Stream" /> containing the attachment data.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="attachmentUrl"/> is null.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<Stream> DownloadAttachment(Uri attachmentUrl);

        /// <summary>
        /// Deletes an attachment for an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-issues-issueID-attachments.html#delete-IssueAttachment-method">Delete Attachment from an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to which the attachment belongs.</param>
        /// <param name="attachmentId">Id of the attachment.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="attachmentId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task DeleteAttachmentForIssue(string issueId, string attachmentId);
    }
}