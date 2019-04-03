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
        /// <inheritdoc />
        public async Task AttachFileToIssue(string issueId, string attachmentName, Stream attachmentStream, string group = null, string author = null)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
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

            var content = new MultipartFormDataContent
            {
                streamContent
            };

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync($"rest/issue/{issueId}/attachment?{query}", content);

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

        /// <inheritdoc />
        public async Task<IEnumerable<Attachment>> GetAttachmentsForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/issue/{issueId}/attachment");

            response.EnsureSuccessStatusCode();

            var wrapper = JsonConvert.DeserializeObject<AttachmentCollectionWrapper>(await response.Content.ReadAsStringAsync());
            return wrapper.Attachments;
        }

        /// <inheritdoc />
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

        /// <inheritdoc />
        public async Task DeleteAttachmentForIssue(string issueId, string attachmentId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.DeleteAsync($"rest/issue/{issueId}/attachment/{attachmentId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return;
            }

            response.EnsureSuccessStatusCode();
        }
    }
}