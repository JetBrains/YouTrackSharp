using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                var response = await client.VisibilityGroupsPostAsync(
                    "groupsWithoutRecommended(id,name),recommendedGroups(id,name)",
                    new VisibilityGroupsRequest()
                    {
                        Prefix = group.ToLower(),
                        Top = 1000000000,
                        Issues = new List<Generated.Issue>() {new Generated.Issue() {IdReadable = issueId}}
                    });
                var userGroup =
                    response.RecommendedGroups.FirstOrDefault(g =>
                        g.Name.Equals(group, StringComparison.InvariantCultureIgnoreCase)) ??
                    response.GroupsWithoutRecommended.FirstOrDefault(g =>
                        g.Name.Equals(group, StringComparison.InvariantCultureIgnoreCase));
                if (userGroup == null)
                {
                    throw new YouTrackErrorException(Strings.Exception_BadRequest, (int)HttpStatusCode.BadRequest,
                        $"Could not suggest visibility group with name {group} for issue {issueId}",
                        null, null);
                }
                attachment.Visibility = new LimitedVisibility() {PermittedGroups = new List<UserGroup> {userGroup}};
            }
            if (!string.IsNullOrEmpty(author))
            {
                attachment.Author = new Me() {Login = author};
            }
            if (!string.IsNullOrEmpty(attachmentContentType))
            {
                attachment.MimeType = attachmentContentType;
            }
            
            var apiAttachment =
                await client.IssuesAttachmentsPostFromStreamAsync(issueId, attachmentStream, "id", attachment);
            //TODO for some reason, group in the body above is ignored
            await client.IssuesAttachmentsPostAsync(issueId, apiAttachment.Id, "id", attachment);
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
                await client.IssuesAttachmentsGetAsync(issueId, Constants.FieldsQueryStrings.ATTACHMENTS, 0, -1);

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
            HttpResponseMessage response;
            if (!attachmentUrl.IsAbsoluteUri)
            {
                var siteUrl = $"{client.BaseAddress.Scheme}://{client.BaseAddress.Authority}";
                response = await client.GetAsync(siteUrl.TrimEnd('/') + "/" + attachmentUrl.ToString().TrimStart('/'));
            }
            else
            {
                response = await client.GetAsync(attachmentUrl);
            }

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