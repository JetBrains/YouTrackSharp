using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace YouTrackSharp.Issues.Interfaces
{
    public interface IIssuesService
    {
        Task ApplyCommand(string issueId, string command, string comment = null, bool disableNotifications = false, string runAs = null);
        Task AttachFileToIssue(string issueId, string attachmentName, Stream attachmentStream, string group = null, string author = null);
        Task<string> CreateIssue(string projectId, Issue issue);
        Task DeleteAttachmentForIssue(string issueId, string attachmentId);
        Task DeleteCommentForIssue(string issueId, string commentId, bool permanent = false);
        Task DeleteIssue(string issueId);
        Task<Stream> DownloadAttachment(Uri attachmentUrl);
        Task<bool> Exists(string issueId);
        Task<IEnumerable<Attachment>> GetAttachmentsForIssue(string issueId);
        Task<IEnumerable<Change>> GetChangeHistoryForIssue(string issueId);
        Task<IEnumerable<Comment>> GetCommentsForIssue(string issueId, bool wikifyDescription = false);
        Task<Issue> GetIssue(string issueId, bool wikifyDescription = false);
        Task<long> GetIssueCount(string filter = null);
        Task<ICollection<Issue>> GetIssues(string filter = null, int? skip = null, int? take = null, bool wikifyDescription = false);
        Task<ICollection<Issue>> GetIssuesInProject(string projectId, string filter = null, int? skip = null, int? take = null, DateTime? updatedAfter = null, bool wikifyDescription = false);
        Task<IEnumerable<Link>> GetLinksForIssue(string issueId);
        Task UpdateCommentForIssue(string issueId, string commentId, string text);
        Task UpdateIssue(string issueId, string summary = null, string description = null, bool? isMarkdown = null);
    }
}