using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using YouTrackSharp.TimeTracking;

namespace YouTrackSharp.AgileBoards
{
    public class AgileBoardService
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

        /// <summary>
        /// Creates an agile board on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Create-New-Agile-Configuration.html">Create New Agile Configuration</a>.
        /// Note that the data contained in the <paramref name="agileSettings"/> needs to be accurate i.e. any projects referenced should exist on the server.</remarks>
        /// <example>
        /// This sample shows how to create an Agile Board. The name property is mandatory and YouTrack will return a <see cref="T:System.Net.HttpRequestException"/> if the
        /// property is not set. Omitting Projects or ColumnSettings will create a board that has configuration errors.
        /// <code>
        /// var connection = new BearerTokenConnection("youtrack url", "some token");
        /// var service = connection.CreateAgileBoardsService();
        /// 
        /// var projects = new List<Project> { new Project { Id = "TP" } };
        /// var columnSettings = new ColumnSettings
        /// {
        ///     Field = new Field { Name = "State" }
        /// };
        /// var agileSettings = new AgileSettings
        /// {
        ///     Name = "Test Board",
        ///     Projects = projects,
        ///     ColumnSettings = columnSettings
        /// 
        /// };
        /// 
        /// string newBoardId = await service.CreateAgileBoard(agileSettings);
        /// </code>
        /// </example>
        /// <param name="agileSettings">The <see cref="AgileSettings"/> to create.</param>
        /// <returns>The newly created <see cref="AgileSettings" />'s id on the server.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="agileSettings"/> is null.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<string> CreateAgileBoard(AgileSettings agileSettings)
        {
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
                else
                {
                    throw new YouTrackErrorException(Strings.Exception_UnknownError);
                }
            }

            response.EnsureSuccessStatusCode();

            // Extract work item id from Location header response
            const string marker = "admin/agile/";
            var locationHeader = response.Headers.Location.ToString();

            return locationHeader.Substring(locationHeader.IndexOf(marker, StringComparison.OrdinalIgnoreCase) + marker.Length);
        }

        /// <summary>
        /// Get a list of agile boards
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-List-of-Agile-Boards.html">Get the List of Agile Boards</a>.</remarks>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="AgileSettings" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<AgileSettings>> GetAgileBoards()
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync("rest/admin/agile");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<ICollection<AgileSettings>>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Get the agile board with the specified id.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Agile-Configuration-by-ID.html">Get Agile Configuration by ID</a>.</remarks>
        ///  <param name="agileBoardId">Id of the agile board containing the sprint.</param>
        /// <returns>An <see cref="AgileSettings" /> that match the specified parameter.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<AgileSettings> GetAgileBoard(string agileBoardId)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/agile/{agileBoardId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<AgileSettings>(await response.Content.ReadAsStringAsync());
        }

        /// <summary>
        /// Get sprint by id
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Sprint-by-ID.html">Get Sprint by ID</a>.</remarks>
        ///  <param name="agileBoardId">Id of the agile board containing the sprint.</param>
        ///  <param name="sprintId">Id of the sprint.</param>
        /// <returns>A <see cref="Sprint" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Sprint> GetSprint(string agileBoardId, string sprintId)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"rest/admin/agile/{agileBoardId}/sprint/{sprintId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<Sprint>(await response.Content.ReadAsStringAsync());
        }
    }
}