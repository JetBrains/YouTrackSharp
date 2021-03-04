using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTrackSharp.Agiles {
  public interface IAgileService {
    /// <summary>
    /// Retrieves the available agile boards from the server.
    /// </summary>
    /// <param name="verbose">
    /// If the full representation of agile boards should be returned.
    /// If this parameter is <c>false</c>, all the fields (and sub-fields) marked with the
    /// <see cref="YouTrackSharp.SerializationAttributes.VerboseAttribute"/> are omitted (for more information, see
    /// <see cref="Agile"/> and related classes).
    /// </param>
    /// <remarks>
    /// Uses the REST API
    /// <a href="https://www.jetbrains.com/help/youtrack/standalone/resource-api-agiles.html#get_all-Agile-method">
    /// Read a list of Agiles
    /// </a>
    /// </remarks>
    /// <returns>A <see cref="ICollection{T}"/> of available <see cref="Agile"/> boards</returns>
    /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
    public Task<ICollection<Agile>> GetAgileBoards(bool verbose = false);
  }
}