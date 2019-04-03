using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}/comment?wikifyDescription={wikifyDescription}");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<IEnumerable<Comment>>(await response.Content.ReadAsStringAsync());
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

            var payload = new JObject(new JProperty("text", text));
            var content = new StringContent(payload.ToString(Formatting.None));
            content.Headers.ContentType = new MediaTypeHeaderValue(Constants.HttpContentTypes.ApplicationJson);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PutAsync($"rest/issue/{issueId}/comment/{commentId}", content);

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
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