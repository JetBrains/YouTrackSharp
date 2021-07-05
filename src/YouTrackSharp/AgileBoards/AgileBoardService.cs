using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace YouTrackSharp.AgileBoards
{
    public class AgileBoardService : IAgileBoardService
    {
        private readonly Connection _connection;

        /// <summary>
        /// Creates an instance of the <see cref="AgileBoardService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public AgileBoardService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }

        /// <inheritdoc />
        public async Task<string> CreateAgileBoard(AgileSettings agileSettings)
        {
            throw new NotImplementedException();
            /*
            if (agileSettings == null)
            {
                throw new ArgumentNullException(nameof(agileSettings));
            }

            var stringContent = new StringContent(JsonConvert.SerializeObject(agileSettings));
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(Constants.HttpContentTypes.ApplicationJson);

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PostAsync("rest/admin/agile", stringContent);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Try reading the error message
                var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                if (responseJson["value"] != null)
                {
                    throw new YouTrackErrorException(responseJson["value"].Value<string>());
                }

                throw new YouTrackErrorException(Strings.Exception_UnknownError);
            }

            response.EnsureSuccessStatusCode();

            // Extract work item id from Location header response
            const string marker = "admin/agile/";
            var locationHeader = response.Headers.Location.ToString();

            return locationHeader.Substring(locationHeader.IndexOf(marker, StringComparison.OrdinalIgnoreCase) + marker.Length);
            */
        }

        /// <inheritdoc />
        public async Task<ICollection<AgileSettings>> GetAgileBoards()
        {
            throw new NotImplementedException();
            /*
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync("rest/admin/agile");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<ICollection<AgileSettings>>(await response.Content.ReadAsStringAsync());
            */
        }

        /// <inheritdoc />
        public async Task<AgileSettings> GetAgileBoard(string agileBoardId)
        {
            throw new NotImplementedException();
            /*
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/agile/{agileBoardId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<AgileSettings>(await response.Content.ReadAsStringAsync());
            */
        }

        /// <inheritdoc />
        public async Task UpdateAgileBoard(string agileBoardId, AgileSettings agileSettings)
        {
            throw new NotImplementedException();
            /*
            if (agileSettings == null)
            {
                throw new ArgumentNullException(nameof(agileSettings));
            }

            // Ensure no null values or empty collections are sent
            if (agileSettings.Projects?.Count == 0) agileSettings.Projects = null;
            if (agileSettings.Sprints?.Count == 0) agileSettings.Sprints = null;

            var stringContent = new StringContent(JsonConvert.SerializeObject(agileSettings, 
                new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                }));
            stringContent.Headers.ContentType = new MediaTypeHeaderValue(Constants.HttpContentTypes.ApplicationJson);

            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.PutAsync($"rest/admin/agile/{agileBoardId}", stringContent);

            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                // Try reading the error message
                var responseJson = JObject.Parse(await response.Content.ReadAsStringAsync());
                if (responseJson["value"] != null)
                {
                    throw new YouTrackErrorException(responseJson["value"].Value<string>());
                }

                throw new YouTrackErrorException(Strings.Exception_UnknownError);
            }

            response.EnsureSuccessStatusCode();
            */
        }

        /// <inheritdoc />
        public async Task<Sprint> GetSprint(string agileBoardId, string sprintId)
        {
            throw new NotImplementedException();
            /*
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/agile/{agileBoardId}/sprint/{sprintId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<Sprint>(await response.Content.ReadAsStringAsync());
            */
        }
    }
}