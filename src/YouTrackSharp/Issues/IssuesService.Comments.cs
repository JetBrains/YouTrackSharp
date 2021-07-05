using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Issues
{
    public partial class IssuesService
    {
        /// <inheritdoc />
        public async Task<IEnumerable<Comment>> GetCommentsForIssue(string issueId, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedApiClient();
            var response = await client.IssuesCommentsGetAsync(issueId, COMMENTS_FIELDS_QUERY, 0, -1);

            return response.Select(comment => Comment.FromApiEntity(comment, wikifyDescription));
        }

        /// <inheritdoc />
        public async Task UpdateCommentForIssue(string issueId, string commentId, string text)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            if (string.IsNullOrEmpty(commentId))
            {
                throw new ArgumentNullException(nameof(commentId));
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentNullException(nameof(text));
            }

            var client = await _connection.GetAuthenticatedApiClient();
            //TODO text could be too large to send w/o multipart, generated api needs to be checked
            await client.IssuesCommentsPostAsync(issueId, commentId, "id", new IssueComment() {Text = text});
        }

        /// <inheritdoc />
        public async Task DeleteCommentForIssue(string issueId, string commentId, bool permanent = false)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            if (string.IsNullOrEmpty(commentId))
            {
                throw new ArgumentNullException(nameof(commentId));
            }
            
            var client = await _connection.GetAuthenticatedApiClient();
            if (permanent)
            {
                try
                {
                    await client.IssuesCommentsDeleteAsync(issueId, commentId);
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
            else
            {
                await client.IssuesCommentsPostAsync(issueId, commentId, "id", new IssueComment() {Deleted = true});
            }
        }
    }
}