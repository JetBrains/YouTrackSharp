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
        /// <summary>
        /// Get issues in a project from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Issues-in-a-Project.html">Get Issues in a Project</a>.</remarks>
        /// <param name="projectId">Id of a project to get issues from.</param>
        /// <param name="filter">Apply a filter to issues in a project.</param>
        /// <param name="skip">The number of issues to skip before getting a list of issues.</param>
        /// <param name="take">Maximum number of issues to be returned. Defaults to the server-side default of the YouTrack server instance..</param>
        /// <param name="updatedAfter">Only issues updated after the specified date will be retrieved.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then issue description in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
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
                queryString.Add($"filter={WebUtility.UrlEncode(filter)}");
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

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/byproject/{projectId}?{query}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<ICollection<Issue>>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Get issues from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-the-List-of-Issues.html">Get the List of Issues</a>.</remarks>
        /// <param name="filter">Apply a filter to issues.</param>
        /// <param name="skip">The number of issues to skip before getting a list of issues.</param>
        /// <param name="take">Maximum number of issues to be returned. Defaults to the server-side default of the YouTrack server instance.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then issue description in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<Issue>> GetIssues(string filter = null, int? skip = null, int? take = null, bool wikifyDescription = false)
        {
            var queryString = new List<string>(4);
            if (!string.IsNullOrEmpty(filter))
            {
                queryString.Add($"filter={WebUtility.UrlEncode(filter)}");
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

            var res = await response.Content.ReadAsStringAsync();
            var wrapper = JsonConvert.DeserializeObject<IssueCollectionWrapper>(res);
            return wrapper.Issues;
        }

        /// <summary>
        /// Get issue count from the server. This operation may be retried internally and take a while to complete.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-a-Number-of-Issues.html">Get a Number of Issues</a>.</remarks>
        /// <param name="filter">Apply a filter to issues.</param>
        /// <returns>The number of <see cref="Issue" /> that match the specified filter.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<long> GetIssueCount(string filter = null)
        {
            var query = !string.IsNullOrEmpty(filter)
                ? $"filter={WebUtility.UrlEncode(filter)}"
                : string.Empty;

            var client = await _connection.GetAuthenticatedHttpClient();

            var retryPolicy = new LinearRetryPolicy<long>(async () =>
                {
                    var response = await client.GetAsync($"rest/issue/count?{query}");

                    if (response.StatusCode == HttpStatusCode.NotFound)
                    {
                        return default(long);
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