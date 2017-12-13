using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;

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
        /// Get a list of agile boards
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-List-of-Agile-Boards.html">Get the List of Agile Boards</a>.</remarks>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="AgileSettings" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<ICollection<AgileSettings>> GetAgileBoards()
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"/rest/admin/agile");

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
        ///  <param name="agileId">Id of the agile board containing the sprint.</param>
        /// <returns>An <see cref="AgileSettings" /> that match the specified parameter.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<AgileSettings> GetAgileBoard(string agileId)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"/rest/admin/agile/{agileId}");

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
        ///  <param name="agileId">Id of the agile board containing the sprint.</param>
        ///  <param name="sprintId">Id of the sprint.</param>
        /// <returns>A <see cref="Sprint" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        public async Task<Sprint> GetSprint(string agileId, string sprintId)
        {
            var client = await _connection.GetAuthenticatedHttpClient();
            var response = await client.GetAsync($"/rest/admin/agile/{agileId}/sprint/{sprintId}");

            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return null;
            }

            response.EnsureSuccessStatusCode();

            return JsonConvert.DeserializeObject<Sprint>(await response.Content.ReadAsStringAsync());
        }
    }
}