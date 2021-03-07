using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using YouTrackSharp.Internal;

namespace YouTrackSharp.Agiles
{
    /// <summary>
    /// Service offering Agile-related operations. 
    /// </summary>
    public class AgileService : IAgileService
    {
        private readonly Connection _connection;

        private readonly FieldSyntaxEncoder _fieldSyntaxEncoder;

        /// <summary>
        /// Creates an instance of the <see cref="AgileService"/> class.
        /// </summary>
        /// <param name="connection">
        /// A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.
        /// </param>
        /// <param name="fieldSyntaxEncoder">
        /// An <see cref="FieldSyntaxEncoder"/> instance that allows to encode types into Youtrack request URL format for fields 
        /// </param>
        public AgileService(Connection connection, FieldSyntaxEncoder fieldSyntaxEncoder)
        {
            _connection = connection;
            _fieldSyntaxEncoder = fieldSyntaxEncoder;
        }

        /// <inheritdoc />
        public async Task<ICollection<Agile>> GetAgileBoards(bool verbose = false)
        {
            HttpClient client = await _connection.GetAuthenticatedHttpClient();

            const int batchSize = 10;
            List<Agile> agileBoards = new List<Agile>();
            List<Agile> currentBatch;
            
            do
            {
                string fields = _fieldSyntaxEncoder.Encode(typeof(Agile), verbose);

                HttpResponseMessage message = await client.GetAsync($"api/agiles?fields={fields}&$top={batchSize}&$skip={agileBoards.Count}");

                string response = await message.Content.ReadAsStringAsync();

                currentBatch = JsonConvert.DeserializeObject<List<Agile>>(response);

                agileBoards.AddRange(currentBatch);
            } while (currentBatch.Count == batchSize);

            return agileBoards;
        }
    }
}