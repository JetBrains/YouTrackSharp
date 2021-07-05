using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Issues
{
    public partial class IssuesService
    {
        /// <inheritdoc />
        public async Task AttachFileToIssue(string issueId, string attachmentName, Stream attachmentStream, string group = null, string author = null, string attachmentContentType = null)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            if (string.IsNullOrEmpty(attachmentName))
            {
                throw new ArgumentNullException(nameof(attachmentName));
            }
            if (attachmentStream == null)
            {
                throw new ArgumentNullException(nameof(attachmentStream));
            }
            
            var client = await _connection.GetAuthenticatedApiClient();

            var attachment = new IssueAttachment();
            if (!string.IsNullOrEmpty(attachmentName))
            {
                attachment.Name = attachmentName;
            }
            if (!string.IsNullOrEmpty(group))
            {
                var response = await client.GroupsGetAsync("id,name", 0, -1);
                var userGroup = response.First(g => g.Name == group);
                attachment.Visibility = group == "All Users"
                    ? new UnlimitedVisibility()
                    : new LimitedVisibility() {PermittedGroups = new List<UserGroup> {userGroup}};
            }
            if (!string.IsNullOrEmpty(author))
            {
                attachment.Author = new Me() {Login = author};
            }
            if (!string.IsNullOrEmpty(attachmentContentType))
            {
                attachment.MimeType = attachmentContentType;
            }
            
            await client.IssuesAttachmentsPostFromStreamAsync(issueId, attachmentStream, "id", attachment);
        }

        /// <inheritdoc />
        public async Task<IEnumerable<Attachment>> GetAttachmentsForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedApiClient();
            var response =
                await client.IssuesAttachmentsGetAsync(issueId, "id,url,name,author(login),visibility(permittedGroups(name)),created", 0, -1);

            return response.Select(Attachment.FromApiEntity);
        }

        /// <inheritdoc />
        public async Task<Stream> DownloadAttachment(Uri attachmentUrl)
        {
            if (attachmentUrl == null)
            {
                throw new ArgumentNullException(nameof(attachmentUrl));
            }

            var client = await _connection.GetAuthenticatedRawClient();
            var response = await client.GetAsync(attachmentUrl);
            
            //TODO some handling is required here

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAttachmentForIssue(string issueId, string attachmentId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedApiClient();
            
            try
            {
                await client.IssuesAttachmentsDeleteAsync(issueId, attachmentId);
            }
            catch (YouTrackErrorException e)
            {
                if (e.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    return;
                }

                throw;
            }
        }
    }
}