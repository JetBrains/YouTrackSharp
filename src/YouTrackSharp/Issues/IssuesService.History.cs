using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace YouTrackSharp.Issues
{
    public partial class IssuesService
    {
        /// <inheritdoc />
        public async Task<IEnumerable<Change>> GetChangeHistoryForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}/changes");

            response.EnsureSuccessStatusCode();

            var wrapper = JsonConvert.DeserializeObject<ChangeCollectionWrapper>(await response.Content.ReadAsStringAsync());
            return wrapper.Changes;
        }
    }
}