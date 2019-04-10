using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.AgileBoards
{
    public partial class AgileBoardServiceTests
    {
        public class GetSprint
        {
            [Fact(Skip = "YouTrack 2018.2 and higher no longer support the agile board API. Please check https://github.com/JetBrains/YouTrackSharp/issues/81 for more information.")]
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

            [Fact(Skip = "YouTrack 2018.2 and higher no longer support the agile board API. Please check https://github.com/JetBrains/YouTrackSharp/issues/81 for more information.")]
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
