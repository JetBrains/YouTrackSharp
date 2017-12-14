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
        public class GetSprint
        {
            [Fact]
            public async Task Valid_Connection_Returns_Sprint()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateAgileBoardService();

                // Act
                var demoSprint = await service.GetSprint(DemoBoardId, DemoSprintId);

                // Assert
                Assert.NotNull(demoSprint);

                Assert.Equal(demoSprint.Id, DemoSprintId);
                Assert.Equal(demoSprint.Version, DemoSprintName);
                Assert.NotNull(demoSprint.Id);
                Assert.NotNull(demoSprint.Version);
            }

            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateAgileBoardService();

                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetSprint(DemoBoardId, DemoSprintId));
            }
        }
    }
}
