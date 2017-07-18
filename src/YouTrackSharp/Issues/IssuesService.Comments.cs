using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    public partial class IssuesService
    {
        /// <summary>
        /// Get comments for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Comments-of-an-Issue.html">Get Comments of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get comments for.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then comments in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Comment" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<IEnumerable<Comment>> GetCommentsForIssue(string issueId, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}/comment?wikifyDescription={wikifyDescription}");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<IEnumerable<Comment>>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Deletes a comment for an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Remove-a-Comment-for-an-Issue.html">Remove a Comment for an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to which the comment belongs.</param>
        /// <param name="commentId">Id of the comment.</param>
        /// <param name="permanent">When <value>true</value>, the specified comment will be deleted permanently and can not be restored afterwards. Defaults to <value>false</value>.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="commentId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task DeleteCommentForIssue(string issueId, string commentId, bool permanent = false)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.DeleteAsync($"rest/issue/{issueId}/comment/{commentId}?permanently={permanent.ToString().ToLowerInvariant()}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }
    }
}