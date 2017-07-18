using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/standalone/Issues-Related-Methods.html">YouTrack Issues Related Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class IssuesService
    {
        private readonly Connection _connection;
        
        private static readonly string[] ReservedFields = new []
        {
            "id", "entityid", "jiraid", "summary", "description"
        };

        /// <summary>
        /// Creates an instance of the <see cref="IssuesService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public IssuesService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <summary>
        /// Get a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-an-Issue.html">Get an Issue</a>.</remarks>
        /// <param name="id">Id of an issue to get.</param>
        /// <param name="wikifyDescription">If set to <value>true</value>, then issue description in the response will be formatted ("wikified"). Defaults to <value>false</value>.</param>
        /// <returns>The <see cref="Issue" /> that matches the requested <paramref name="id"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="id"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Issue> GetIssue(string id, bool wikifyDescription = false)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{id}?wikifyDescription={wikifyDescription}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();
            
            return JsonConvert.DeserializeObject<Issue>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Checks whether an issue exists on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Check-that-an-Issue-Exists.html">Check that an Issue Exists</a>.</remarks>
        /// <param name="id">Id of an issue to check.</param>
        /// <returns><value>True</value> if the issue exists, otherwise <value>false</value>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="id"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<bool> Exists(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{id}/exists");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }

            response.EnsureSuccessStatusCode();

            return true;
        }
        
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
        public async Task<ICollection<Issue>> GetIssuesInProject(string projectId, string filter = null, int? skip = null, int? take = null, DateTime? updatedAfter = null, bool wikifyDescription = false)
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
        /// <param name="take">Maximum number of issues to be returned. Defaults to the server-side default of the YouTrack server instance..</param>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Issue" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<Issue>> GetIssues(string filter = null, int? skip = null, int? take = null)
        {
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
            
            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue?{query}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            var wrapper = JsonConvert.DeserializeObject<IssueCollectionWrapper>(await response.Content.ReadAsStringAsync());
            return wrapper.Issues;
        }

        /// <summary>
        /// Creates an issue on the server in a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Create-New-Issue.html">Create New Issue</a>.</remarks>
        /// <param name="projectId">Id of the project to create an issue in.</param>
        /// <param name="issue">The <see cref="Issue" /> to create. At the minimum needs the Summary field populated.</param>
        /// <returns>The newly created <see cref="Issue" />'s id on the server.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<string> CreateIssue(string projectId, Issue issue)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }
            
            var queryString = new List<string>(3);
            queryString.Add($"project={projectId}");
            
            if (!string.IsNullOrEmpty(issue.Summary))
            {
                queryString.Add($"summary={WebUtility.UrlEncode(issue.Summary)}");
            }
            if (!string.IsNullOrEmpty(issue.Description))
            {
                queryString.Add($"description={WebUtility.UrlEncode(issue.Description)}");
            }
            
            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PutAsync($"rest/issue?{query}", new MultipartFormDataContent());

            response.EnsureSuccessStatusCode();
            
            // Extract issue id from Location header response
            var marker = "rest/issue/";
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
                else
                {
                    await ApplyCommand(issueId, $"{customField.Key} {customField.Value}", string.Empty);
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

        /// <summary>
        /// Updates an issue on the server in a specific project.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Update-an-Issue.html">Update an Issue</a>.</remarks>
        /// <param name="id">Id of the issue to update.</param>
        /// <param name="summary">Updated summary of the issue.</param>
        /// <param name="description">Updated description of the issue.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="id"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task UpdateIssue(string id, string summary = null, string description = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }

            if (summary == null && description == null)
            {
                return;
            }
            
            var queryString = new List<string>(2);
            if (!string.IsNullOrEmpty(summary))
            {
                queryString.Add($"summary={WebUtility.UrlEncode(summary)}");
            }
            if (!string.IsNullOrEmpty(description))
            {
                queryString.Add($"description={WebUtility.UrlEncode(description)}");
            }
            
            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/issue/{id}?{query}", new MultipartFormDataContent());

            response.EnsureSuccessStatusCode();
        }
        
        /// <summary>
        /// Applies a command to a specific issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Apply-Command-to-an-Issue.html">Apply Command to an Issue</a>.</remarks>
        /// <param name="id">Id of the issue to apply the command to.</param>
        /// <param name="command">The command to apply. A command might contain a string of attributes and their values - you can change multiple fields with one complex command.</param>
        /// <param name="comment">A comment to add to an issue.</param>
        /// <param name="disableNotifications">When <value>true</value>, no notifications about changes made with the specified command will be sent. Defaults to <value>false</value>.</param>
        /// <param name="runAs">Login name for a user on whose behalf the command should be applied.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="id"/> or <paramref name="command"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task ApplyCommand(string id, string command, string comment = null, bool disableNotifications = false, string runAs = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            if (string.IsNullOrEmpty(command))
            {
                throw new ArgumentNullException(nameof(command));
            }
            
            var queryString = new List<string>(4);
            queryString.Add($"command={WebUtility.UrlEncode(command)}");
            
            if (!string.IsNullOrEmpty(comment))
            {
                queryString.Add($"comment={WebUtility.UrlEncode(comment)}");
            }
            if (disableNotifications)
            {
                queryString.Add($"disableNotifications=true");
            }
            if (!string.IsNullOrEmpty(runAs))
            {
                queryString.Add($"runAs={runAs}");
            }
            
            var query = string.Join("&", queryString);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/issue/{id}/execute?{query}", new MultipartFormDataContent());
            
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Try reading the error message
                var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                if (responseJson["value"] != null)
                {
                    throw new YouTrackErrorException(responseJson["value"].Value<string>());
                }
                else
                {
                    throw new YouTrackErrorException(Strings.Exception_UnknownError);
                }
            }
            
            response.EnsureSuccessStatusCode();
        }
        
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

        /// <summary>
        /// Attaches a file to an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Attach-File-to-an-Issue.html">Attach File to an Issue</a>.</remarks>
        /// <param name="id">Id of the issue to attach the file to.</param>
        /// <param name="attachmentName">Filename for the attachment.</param>
        /// <param name="attachmentStream">The <see cref="T:System.IO.Stream"/> to attach.</param>
        /// <param name="group">Attachment visibility group.</param>
        /// <param name="author">Creator of the attachment. Note to define author the 'Low-Level Administration' permission is required.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="id"/>, <paramref name="attachmentName"/> or <paramref name="attachmentStream"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task AttachFileToIssue(string id, string attachmentName, Stream attachmentStream, string group = null, string author = null)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            if (string.IsNullOrEmpty(attachmentName))
            {
                throw new ArgumentNullException(nameof(attachmentName));
            }
            if (attachmentStream == null)
            {
                throw new ArgumentNullException(nameof(attachmentStream));
            }

            var queryString = new List<string>(3);
            if (!string.IsNullOrEmpty(attachmentName))
            {
                queryString.Add($"attachmentName={attachmentName}");
            }
            if (!string.IsNullOrEmpty(group))
            {
                queryString.Add($"group={group}");
            }
            if (!string.IsNullOrEmpty(group))
            {
                queryString.Add($"author={author}");
            }
            
            var query = string.Join("&", queryString);

            var streamContent = new StreamContent(attachmentStream);
            streamContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
            {
                FileName = attachmentName,
                Name = attachmentName
            };
            
            var content = new MultipartFormDataContent();
            content.Add(streamContent);
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/issue/{id}/attachment?{query}", content);
            
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Try reading the error message
                var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                if (responseJson["value"] != null)
                {
                    throw new YouTrackErrorException(responseJson["value"].Value<string>());
                }
                else
                {
                    throw new YouTrackErrorException(Strings.Exception_UnknownError);
                }
            }
            
            response.EnsureSuccessStatusCode();
        }
        
        /// <summary>
        /// Get attachments for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Attachments-of-an-Issue.html">Get Attachments of an Issue</a>.</remarks>
        /// <param name="id">Id of the issue to get comments for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="Attachment" /> for the requested issue <paramref name="id"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="id"/> is null or empty.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<IEnumerable<Attachment>> GetAttachmentsForIssue(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                throw new ArgumentNullException(nameof(id));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{id}/attachment");

            response.EnsureSuccessStatusCode();
            
            var wrapper = JsonConvert.DeserializeObject<AttachmentCollectionWrapper>(await response.Content.ReadAsStringAsync());
            return wrapper.Attachments;
        }
        
        /// <summary>
        /// Downloads an attachment from the server.
        /// </summary>
        /// <param name="attachmentUrl">The <see cref="T:System.Uri" /> of the attachment.</param>
        /// <returns>A <see cref="T:System.IO.Stream" /> containing the attachment data.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="attachmentUrl"/> is null.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Stream> DownloadAttachment(Uri attachmentUrl)
        {
            if (attachmentUrl == null)
            {
                throw new ArgumentNullException(nameof(attachmentUrl));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync(attachmentUrl);

            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStreamAsync();
        }
    }
}