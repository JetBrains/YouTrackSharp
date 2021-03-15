using System;
using System.Collections.Generic;
using System.Linq;
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
            [Fact]
            public async Task Mock_Connection_Return_Many_Agiles_In_Batches()
            {
                // Arrange
                const int totalAgileCount = 53;
                int expectedRequests = (int)Math.Ceiling(totalAgileCount / 10.0);
                
                string[] jsonStrings = Enumerable.Range(0, totalAgileCount).Select(i => FullAgile01).ToArray();
                JsonArrayHandler handler = new JsonArrayHandler(jsonStrings);
                ConnectionStub connection = new ConnectionStub(handler);
                IAgileService agileService = connection.CreateAgileService();

                // Act
                ICollection<Agile> result = await agileService.GetAgileBoards(true);

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);
                Assert.Equal(totalAgileCount, result.Count);
                Assert.Equal(expectedRequests, handler.RequestsReceived);
            }
        }
    }
}