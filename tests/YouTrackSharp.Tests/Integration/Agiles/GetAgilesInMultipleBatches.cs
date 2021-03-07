using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Agiles;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Agiles
{
    public partial class AgileServiceTest
    {
        public class GetAgilesInMultipleBatches
        {
            /// <summary>
            /// Creates a JSON array of Agile objects, whose size is determined by the "$top" and "$skip" parameters
            /// of the request (with a maximum of <see cref="totalAgiles"/> - skipped).<br></br>
            /// This allows to simulate returning a total number of agiles, in batches (the size of the batch is
            /// determined by the <see cref="AgileService"/> itself. 
            /// </summary>
            /// <param name="request">REST request, with the $top parameter indicating the max number of results</param>
            /// <param name="totalAgiles">Total number of agiles to simulate in the server</param>
            /// <returns>
            /// Json array of agiles, whose size is hose size is determined by the "$top" and "$skip" parameters of the
            /// request (with a maximum of <see cref="totalAgiles"/> - skipped)
            /// </returns>
            private HttpResponseMessage GetAgileBatch(HttpRequestMessage request, int totalAgiles)
            {
                string requestUri = request.RequestUri.ToString();

                Match match = Regex.Match(requestUri, "(&\\$top=(?<top>[0-9]+))?(&\\$skip=(?<skip>[0-9]+))?");

                int top = totalAgiles;
                if (match.Groups.ContainsKey("top") && match.Groups["top"].Success)
                {
                    top = int.Parse(match.Groups["top"].Value);
                }

                int skip = 0;
                if (match.Groups.ContainsKey("skip") && match.Groups["skip"].Success)
                {
                    skip = int.Parse(match.Groups["skip"].Value);
                }

                int batchSize = Math.Min(top, totalAgiles - skip);
                
                string agileJsonArray = GetAgileJsonArray(batchSize);
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new StringContent(agileJsonArray);

                return response;
            }

            [Fact]
            public async Task Many_Agiles_Are_Fetched_In_Batches()
            {
                // Arrange
                const int totalAgileCount = 53;
                Connection connection = new ConnectionStub(request => GetAgileBatch(request, totalAgileCount));
                IAgileService agileService = connection.CreateAgileService();

                // Act
                ICollection<Agile> result = await agileService.GetAgileBoards(true);

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.Equal(totalAgileCount, result.Count);
            }
        }
    }
}