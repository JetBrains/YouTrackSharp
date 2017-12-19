using System.Net.Http;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.AgileBoards;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.AgileBoards
{
    public partial class AgileBoardServiceTests
    {
        public class CreateAgileBoard
        {
            /*
             * It is not feasible at this point to write a unit test for the creation of a board 
             * since there is no way to remove it automatically after the test has run. 
             */

            [Fact]
            public async Task Valid_Connection_Throws_HttpRequestException_On_Bad_Parameter()
            {
                // Arrange
                var service = Connections.Demo1Token.CreateAgileBoardService();

                // Act & Assert
                await Assert.ThrowsAsync<HttpRequestException>(
                    async () => await service.CreateAgileBoard(new AgileSettings()));
            }

            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateAgileBoardService();

                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.CreateAgileBoard(new AgileSettings { Name = "Test Board" }));
            }
        }
    }
}
