using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.AgileBoards
{
    public partial class AgileBoardServiceTests
    {
        public class GetAgileBoard
        {
            [Fact]
            public async Task Valid_Connection_Returns_Agile_Board()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateAgileBoardService();

                // Act
                var demoBoard = await service.GetAgileBoard(DemoBoardId);

                // Assert
                Assert.NotNull(demoBoard);

                Assert.Equal(demoBoard.Id, DemoBoardId);
                Assert.Equal(demoBoard.Name, DemoBoardName);
                Assert.NotNull(demoBoard.Id);
                Assert.NotNull(demoBoard.Name);
                Assert.NotNull(demoBoard.ColumnSettings);
                Assert.NotNull(demoBoard.Backlog);
                Assert.NotNull(demoBoard.Projects);
                Assert.NotNull(demoBoard.Sprints);
            }

            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateAgileBoardService();

                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetAgileBoard(DemoBoardId));
            }
        }
    }
}
