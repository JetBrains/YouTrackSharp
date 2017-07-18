using System;
using System.Collections.Generic;
using System.IO;
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
        public async Task AttachFileToIssue(string id, string attachmentName, Stream attachmentStream,
            string group = null, string author = null)
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
            if (!string.IsNullOrEmpty(@group))
            {
                queryString.Add($"group={@group}");
            }
            if (!string.IsNullOrEmpty(@group))
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

            var wrapper =
                JsonConvert.DeserializeObject<AttachmentCollectionWrapper>(await response.Content.ReadAsStringAsync());
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