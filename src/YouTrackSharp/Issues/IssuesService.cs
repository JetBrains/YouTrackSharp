using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Issues-Related-Methods.html">YouTrack Issues Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public partial class IssuesService : IIssuesService
    {
        private readonly Connection _connection;
        
        private static readonly string[] ReservedFields = 
        {
            "id", "entityid", "jiraid", "summary", "description", "markdown"
        };

        /// <summary>
        /// Creates an instance of the <see cref="IssuesService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public IssuesService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc />
        public async Task<Issue> GetIssue(string issueId, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedApiClient();
            try
            {
                //TODO custom fields customFields(value(id,name))
                var response = await client.IssuesGetAsync(issueId, "id,idReadable,usesMarkdown,summary,description,wikifiedDescription,comments(id,text),tags(id,name),customFields(id,name)", default(System.Threading.CancellationToken));
                return Issue.FromApiEntity(response);
            }
            catch (YouTrackErrorException e)
            {
                if (e.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    return null;
                }

                throw;
            }
        }

        /// <inheritdoc />
        public async Task<bool> Exists(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedApiClient();
            try
            {
                //TODO fields
                await client.IssuesGetAsync(issueId,
                    "id,idReadable,usesMarkdown,summary,description,wikifiedDescription,comments(id,text),tags(id,name),customFields(id,name)",
                    default(System.Threading.CancellationToken));
            }
            catch (YouTrackErrorException e)
            {
                if (e.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    return false;
                }
                
                throw;
            }

            return true;
        }

        /// <inheritdoc />
        public async Task<string> CreateIssue(string projectId, Issue issue)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            var queryString = new List<string>(4)
            {
                $"project={projectId}"
            };

            if (!string.IsNullOrEmpty(issue.Summary))
            {
                queryString.Add($"summary={Uri.EscapeDataString(issue.Summary)}");
            }
            if (!string.IsNullOrEmpty(issue.Description))
            {
                queryString.Add($"description={Uri.EscapeDataString(issue.Description)}");
            }
            queryString.Add("markdown=" + (issue.IsMarkdown ? "true" : "false"));
            
            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PutAsync($"rest/issue?{query}", new MultipartFormDataContent());

            response.EnsureSuccessStatusCode();
            
            // Extract issue id from Location header response
            const string marker = "rest/issue/";
            var locationHeader = response.Headers.Location.ToString();
            
            var issueId = locationHeader.Substring(locationHeader.IndexOf(marker, StringComparison.OrdinalIgnoreCase) + marker.Length);

            // For every custom field, apply a command
            var customFields = issue.Fields
                .Where(field => !ReservedFields.Contains(field.Name.ToLower()))
                .ToDictionary(field => field.Name, field => field.Value);
            
            foreach (var customField in customFields)
            {
                if (!(customField.Value is string) && customField.Value is System.Collections.IEnumerable enumerable)
                {
                    await ApplyCommand(issueId, $"{customField.Key} {string.Join(" ", enumerable.OfType<string>())}", string.Empty);
                }
                else switch (customField.Value)
                {
                    case DateTime dateTime:
                        await ApplyCommand(issueId, $"{customField.Key} {dateTime:s}", string.Empty);
                        break;
                    case DateTimeOffset dateTimeOffset:
                        await ApplyCommand(issueId, $"{customField.Key} {dateTimeOffset:s}", string.Empty);
                        break;
                    default:
                        await ApplyCommand(issueId, $"{customField.Key} {customField.Value}", string.Empty);
                        break;
                }
            }
            
            // Add comments?
            foreach (var issueComment in issue.Comments)
            {
                await ApplyCommand(issueId, "comment", issueComment.Text, runAs: issueComment.Author);
            }
            
            // Add tags?
            foreach (var issueTag in issue.Tags)
            {
                await ApplyCommand(issueId, $"tag {issueTag.Value}");
            }
            
            return issueId;
        }

        /// <inheritdoc />
        public async Task UpdateIssue(string issueId, string summary = null, string description = null, bool? isMarkdown = null)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            if (summary == null && description == null)
            {
                return;
            }
            
            var issue = new Generated.Issue();
            
            if (!string.IsNullOrEmpty(summary))
            {
                issue.Summary = summary;
            }
            if (!string.IsNullOrEmpty(description))
            {
                issue.Description = description;
            }
            if (isMarkdown.HasValue)
            {
                issue.UsesMarkdown = isMarkdown;
            }
            
            var client = await _connection.GetAuthenticatedApiClient();
            
            await client.IssuesPostAsync(issueId, "id", issue);
        }
        
        /// <inheritdoc />
        public async Task ApplyCommand(string issueId, string command, string comment = null, bool disableNotifications = false, string runAs = null)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            var commandList = new CommandList()
            {
                Query = command,
                Issues = new List<Generated.Issue>() {new Generated.Issue() {IdReadable = issueId}}
            };
            if (!string.IsNullOrEmpty(comment))
            {
                commandList.Comment = comment;
            }
            if (disableNotifications)
            {
                commandList.Silent = true;
            }
            if (!string.IsNullOrEmpty(runAs))
            {
                commandList.RunAs = runAs;
            }
            
            var client = await _connection.GetAuthenticatedApiClient();
            
            await client.CommandsPostAsync("id", commandList);
        }
        
        /// <inheritdoc />
        public async Task DeleteIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedApiClient();
            try
            {
                await client.IssuesDeleteAsync(issueId);
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

        /// <inheritdoc />
        public async Task<IEnumerable<Link>> GetLinksForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}/link");

            response.EnsureSuccessStatusCode();
            
            return JsonConvert.DeserializeObject<IEnumerable<Link>>(await response.Content.ReadAsStringAsync());
        }
    }
}