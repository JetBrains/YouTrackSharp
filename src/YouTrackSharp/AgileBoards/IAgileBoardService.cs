using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTrackSharp.AgileBoards
{
    public interface IAgileBoardService
    {
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
        /// var projects = new List&lt;Project&gt; { new Project { Id = "TP" } };
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
        Task<string> CreateAgileBoard(AgileSettings agileSettings);

        /// <summary>
        /// Get a list of agile boards
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-List-of-Agile-Boards.html">Get the List of Agile Boards</a>.</remarks>
        /// <returns>A <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="AgileSettings" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<ICollection<AgileSettings>> GetAgileBoards();

        /// <summary>
        /// Get the agile board with the specified id.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Agile-Configuration-by-ID.html">Get Agile Configuration by ID</a>.</remarks>
        ///  <param name="agileBoardId">Id of the agile board containing the sprint.</param>
        /// <returns>An <see cref="AgileSettings" /> that match the specified parameter.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<AgileSettings> GetAgileBoard(string agileBoardId);

        /// <summary>
        /// Updates an agile board on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Update-Agile-Configuration.html">Update Agile Configuration</a>.
        /// Note that the data contained in the <paramref name="agileSettings"/> needs to be accurate i.e. any projects referenced should exist on the server.</remarks>
        /// <param name="agileBoardId">Id of the agile board to update.</param>
        /// <param name="agileSettings">The <see cref="AgileSettings"/> to update.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="agileSettings"/> is null.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task UpdateAgileBoard(string agileBoardId, AgileSettings agileSettings);

        /// <summary>
        /// Get sprint by id
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/standalone/Get-Sprint-by-ID.html">Get Sprint by ID</a>.</remarks>
        ///  <param name="agileBoardId">Id of the agile board containing the sprint.</param>
        ///  <param name="sprintId">Id of the sprint.</param>
        /// <returns>A <see cref="Sprint" /> that match the specified parameters.</returns>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<Sprint> GetSprint(string agileBoardId, string sprintId);
    }
}