using System.Collections.Generic;
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
            [Fact(Skip = "Skip test until the API supports removing the created board to avoid cluttering the test environment with hundreds of boards")]
            public async Task Valid_Connection_Creates_Agile_Board()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateAgileBoardService();

                // Act
                var projects = new List<Project> { new Project { Id = "DP1" } };
                var columnSettings = new ColumnSettings
                {
                    Field = new Field { Name = "State" }
                };
                var agileSettings = new AgileSettings
                {
                    Name = "Test Board",
                    Projects = projects,
                    ColumnSettings = columnSettings

                };
                var boardId = await service.CreateAgileBoard(agileSettings);
                var newBoard = await service.GetAgileBoard(boardId);

                // Assert
                Assert.NotNull(boardId);
                Assert.NotNull(newBoard);
                Assert.NotNull(newBoard.Name);
                Assert.NotNull(newBoard.Projects);
                Assert.NotNull(newBoard.ColumnSettings);
                Assert.NotNull(newBoard.ColumnSettings.Field);
                Assert.Equal("Test Board", newBoard.Name);
                Assert.Equal("State", newBoard.ColumnSettings.Field.Name);
                Assert.Contains(newBoard.Projects, p => p.Id == "DP1");

                //Clean up
                // Remove the newly created board (currently not supported)
            }

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
