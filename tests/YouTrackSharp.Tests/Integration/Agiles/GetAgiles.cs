using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;
using YouTrackSharp.Agiles;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Agiles
{
    [UsedImplicitly]
    public partial class AgileServiceTest
    {
        public class GetAgiles
        {
            [Fact]
            public async Task Valid_Connection_Return_Existing_Agiles_Verbose()
            {
                // Arrange
                IAgileService agileService = Connections.Demo1Token.CreateAgileService();

                // Act
                ICollection<Agile> result = await agileService.GetAgileBoards(true);

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);

                Agile demoBoard = result.FirstOrDefault();
                Assert.NotNull(demoBoard);
                Assert.Equal(DemoBoardId, demoBoard.Id);
                Assert.Equal(DemoBoardNamePrefix, demoBoard.Name);
            }

            [Fact]
            public async Task Verbose_Disabled_Returns_Agiles_Non_Verbose()
            {
                // Arrange
                IAgileService agileService = Connections.Demo1Token.CreateAgileService();

                // Act
                ICollection<Agile> result = await agileService.GetAgileBoards(false);

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);

                Agile demoBoard = result.FirstOrDefault();
                Assert.NotNull(demoBoard);
                Assert.Equal(DemoBoardId, demoBoard.Id);
                Assert.Equal(DemoBoardNamePrefix, demoBoard.Name);
            }

            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                IAgileService agileService = Connections.UnauthorizedConnection.CreateAgileService();

                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await agileService.GetAgileBoards());
            }

            [Fact]
            public async Task Full_Agile_Json_Gets_Deserialized_Successfully()
            {
                // Arrange
                IAgileService agileService = Connections.ConnectionStub(GetAgileJsonArray(1)).CreateAgileService();

                // Act
                ICollection<Agile> result = await agileService.GetAgileBoards(true);

                // Assert
                Assert.NotNull(result);
                Assert.NotEmpty(result);

                Agile demoBoard = result.FirstOrDefault();
                Assert.NotNull(demoBoard);
                Assert.Equal(DemoBoardId, demoBoard.Id);
                Assert.Equal(DemoBoardNamePrefix, demoBoard.Name);

                Assert.NotNull(demoBoard.ColumnSettings);
                Assert.NotNull(demoBoard.Projects);
                Assert.NotNull(demoBoard.Sprints);
                Assert.NotNull(demoBoard.Projects);
                Assert.NotNull(demoBoard.Sprints);
                Assert.NotNull(demoBoard.Status);
                Assert.NotNull(demoBoard.ColumnSettings);
                Assert.NotNull(demoBoard.CurrentSprint);
                Assert.NotNull(demoBoard.EstimationField);
                Assert.NotNull(demoBoard.SprintsSettings);
                Assert.NotNull(demoBoard.SwimlaneSettings);
                Assert.NotNull(demoBoard.ColorCoding);
                Assert.NotNull(demoBoard.UpdateableBy);
                Assert.NotNull(demoBoard.VisibleFor);
                Assert.NotNull(demoBoard.OriginalEstimationField);

                Sprint sprint = demoBoard.Sprints.FirstOrDefault();
                Assert.NotNull(sprint);
                Assert.Equal(DemoSprintId, sprint.Id);
                Assert.Equal(DemoSprintName, sprint.Name);
            }
        }
    }
}