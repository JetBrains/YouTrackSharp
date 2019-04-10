using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.AgileBoards
{
    public partial class AgileBoardServiceTests
    {
        public class GetAgileBoard
        {
            [Fact(Skip = "YouTrack 2018.2 and higher no longer support the agile board API. Please check https://github.com/JetBrains/YouTrackSharp/issues/81 for more information.")]
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
                Assert.StartsWith(DemoBoardNamePrefix, demoBoard.Name);
                Assert.NotNull(demoBoard.Id);
                Assert.NotNull(demoBoard.Name);
                Assert.NotNull(demoBoard.ColumnSettings);
                Assert.NotNull(demoBoard.Backlog);
                Assert.NotNull(demoBoard.Projects);
                Assert.NotNull(demoBoard.Sprints);
            }

            [Fact(Skip = "YouTrack 2018.2 and higher no longer support the agile board API. Please check https://github.com/JetBrains/YouTrackSharp/issues/81 for more information.")]
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
