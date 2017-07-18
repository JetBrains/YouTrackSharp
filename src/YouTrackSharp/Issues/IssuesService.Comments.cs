using System;
using System.Collections.Generic;
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
        /// <param name="id">Id of the issue to get comments for.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then comments in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Comment" /> for the requested issue <paramref name="id"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="id"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<IEnumerable<Comment>> GetCommentsForIssue(string id, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{id}/comment?wikifyDescription={wikifyDescription}");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<IEnumerable<Comment>>(await response.Content.ReadAsStringAsync());
        }
    }
}