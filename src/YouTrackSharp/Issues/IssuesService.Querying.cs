using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using YouTrackSharp.Generated;
using YouTrackSharp.Internal;

namespace YouTrackSharp.Issues
{
    public partial class IssuesService
    {
        /// <inheritdoc />
        public async Task<ICollection<Issue>> GetIssuesInProject(string projectId, string filter = null,
            int? skip = null, int? take = null, DateTime? updatedAfter = null,
            bool wikifyDescription = false, bool wikifyContents = false)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            var queryString = "project:" + projectId;
            if (updatedAfter.HasValue)
            {
                queryString += " updated:" + updatedAfter?.ToString("yyyy'-'MM'-'dd'T'HH':'mm':'ss") + " .. *";
            }
            queryString += " " + filter ?? "";

            return await GetIssues(queryString, skip, take, wikifyDescription, wikifyContents);
        }

        /// <inheritdoc />
        public async Task<ICollection<Issue>> GetIssues(string filter = null, int? skip = null, int? take = null,
            bool wikifyDescription = false, bool wikifyContents = false)
        {
            var client = await _connection.GetAuthenticatedApiClient();
            var response = await client.IssuesGetAsync(filter,
                (wikifyDescription ? ISSUES_FIELD_WIKIFIED_DESCRIPTION : ISSUES_FIELD_DESCRIPTION) + "," +
                ISSUES_FIELDS_QUERY_NO_DESCRIPTION, skip, take);
            
            return response.Select(issue => Issue.FromApiEntity(issue, wikifyDescription, wikifyContents)).ToList();
        }

        /// <inheritdoc />
        public async Task<long> GetIssueCount(string filter = null)
        {
            var query = !string.IsNullOrEmpty(filter)
                ? $"filter={Uri.EscapeDataString(filter)}"
                : string.Empty;

            var client = await _connection.GetAuthenticatedApiClient();

            var retryPolicy = new LinearRetryPolicy<long>(async () =>
                {
                    var response = await client.IssuesGetterCountPostAsync("count", new IssueCountRequest(){Query = filter});

                    return response.Count;
                },
                result => Task.FromResult(result < 0),
                TimeSpan.FromSeconds(1),
                30);

            return await retryPolicy.Execute();
        }
    }
}