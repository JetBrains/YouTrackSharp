using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    public partial class IssuesService
    {
        /// <summary>
        /// Get changes for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Historical-Changes-of-an-Issue.html">Get Changes of an Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get changes for.</param>
        /// <returns><see cref="Changeset" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Changeset> GetChangsetForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}/changes");

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<Changeset>(await response.Content.ReadAsStringAsync());
        }
    }
}