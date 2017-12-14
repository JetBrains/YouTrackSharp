using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.AgileBoards
{
    public partial class AgileBoardServiceTests
    {
        public class GetAgileBoards
        {
            [Fact]
            public async Task Valid_Connection_Returns_Agile_Boards()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateAgileBoardService();

                // Act
                var result = await service.GetAgileBoards();

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);

                var demoBoard = result.FirstOrDefault();
                Assert.NotNull(demoBoard);
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
                    async () => await service.GetAgileBoards());
            }
        }
    }
}
