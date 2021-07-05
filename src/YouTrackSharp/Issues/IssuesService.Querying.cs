using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YouTrackSharp.Internal;

namespace YouTrackSharp.Issues
{
    public partial class IssuesService
    {
        /// <inheritdoc />
        public async Task<ICollection<Issue>> GetIssuesInProject(string projectId, string filter = null,
            int? skip = null, int? take = null, DateTime? updatedAfter = null, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            var queryString = new List<string>(6);
            if (!string.IsNullOrEmpty(filter))
            {
                queryString.Add($"filter={Uri.EscapeDataString(filter)}");
            }
            if (skip.HasValue)
            {
                queryString.Add($"after={skip}");
            }
            if (take.HasValue)
            {
                queryString.Add($"max={take}");
            }
            if (updatedAfter.HasValue)
            {
                var offset = new DateTimeOffset(updatedAfter.Value);
                queryString.Add($"updatedAfter={offset.ToUnixTimeMilliseconds()}");
            }

            queryString.Add($"wikifyDescription={wikifyDescription}");

            var query = string.Join("&", queryString);

            var client = await _connection.GetAuthenticatedAPIClient();
            var response = await client.IssuesGetAsync(filter, fields, skip, take);
            var response = await client.GetAsync($"rest/issue/byproject/{projectId}?{query}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<ICollection<Issue>>(await response.Content.ReadAsStringAsync());
        }

        /// <inheritdoc />
        public async Task<ICollection<Issue>> GetIssues(string filter = null, int? skip = null, int? take = null, bool wikifyDescription = false)
        {
            var queryString = new List<string>(4);
            if (!string.IsNullOrEmpty(filter))
            {
                queryString.Add($"filter={Uri.EscapeDataString(filter)}");
            }
            if (skip.HasValue)
            {
                queryString.Add($"after={skip}");
            }
            if (take.HasValue)
            {
                queryString.Add($"max={take}");
            }
            
            queryString.Add($"wikifyDescription={wikifyDescription}");

            var query = string.Join("&", queryString);

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue?{query}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var wrapper = JsonConvert.DeserializeObject<IssueCollectionWrapper>(
                await response.Content.ReadAsStringAsync());
            return wrapper.Issues;
        }

        /// <inheritdoc />
        public async Task<long> GetIssueCount(string filter = null)
        {
            var query = !string.IsNullOrEmpty(filter)
                ? $"filter={Uri.EscapeDataString(filter)}"
                : string.Empty;

            var client = await _connection.GetAuthenticatedAPIClient();

            var retryPolicy = new LinearRetryPolicy<long>(async () =>
                {
                    var response = await client.GetAsync($"rest/issue/count?{query}");
                    response = await client.Issues

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return default;
                    }

                    response.EnsureSuccessStatusCode();

                    return JsonConvert.DeserializeObject<SubValue<long>>(
                        await response.Content.ReadAsStringAsync()).Value;
                },
                result => Task.FromResult(result < 0),
                TimeSpan.FromSeconds(1),
                30);

            return await retryPolicy.Execute();
        }
    }
}