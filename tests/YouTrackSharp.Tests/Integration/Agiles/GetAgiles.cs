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
            public async Task Mock_Connection_Returns_Full_Agiles()
            {
                // Arrange
                string[] strings = { FullAgile01, FullAgile02 };

                JsonArrayHandler handler = new JsonArrayHandler(strings);
                ConnectionStub connection = new ConnectionStub(handler);
                
                IAgileService agileService = connection.CreateAgileService();

                // Act
                ICollection<Agile> result = await agileService.GetAgileBoards(true);

                // Assert
                Assert.NotNull(result);
                Assert.Equal(2, result.Count);

                foreach (Agile agile in result)
                {
                    Assert.NotNull(agile);
                    
                    Assert.True("109-1".Equals(agile.Id) || "109-2".Equals(agile.Id));

                    Assert.NotNull(agile.ColumnSettings);
                    Assert.NotNull(agile.Projects);
                    Assert.NotNull(agile.Sprints);
                    Assert.NotNull(agile.Projects);
                    Assert.NotNull(agile.Sprints);
                    Assert.NotNull(agile.Status);
                    Assert.NotNull(agile.ColumnSettings);
                    Assert.NotNull(agile.CurrentSprint);
                    Assert.NotNull(agile.EstimationField);
                    Assert.NotNull(agile.SprintsSettings);
                    Assert.NotNull(agile.SwimlaneSettings);
                    Assert.NotNull(agile.ColorCoding);
                    Assert.NotNull(agile.UpdateableBy);
                    Assert.NotNull(agile.VisibleFor);
                    Assert.NotNull(agile.OriginalEstimationField);

                    Sprint sprint = agile.Sprints.FirstOrDefault();
                    Assert.NotNull(sprint);
                    Assert.Equal(DemoSprintId, sprint.Id);
                    Assert.Equal(DemoSprintName, sprint.Name);

                    if ("109-1".Equals(agile.Id))
                    {
                        Assert.Equal("Full Board 01", agile.Name);
                        Assert.IsType<FieldBasedColorCoding>(agile.ColorCoding);
                        Assert.IsType<IssueBasedSwimlaneSettings>(agile.SwimlaneSettings);
                        Assert.IsType<CustomFilterField>(((IssueBasedSwimlaneSettings)agile.SwimlaneSettings).Field);    
                    }
                    else
                    {
                        Assert.Equal("Full Board 02", agile.Name);
                        Assert.IsType<ProjectBasedColorCoding>(agile.ColorCoding);
                        Assert.IsType<AttributeBasedSwimlaneSettings>(agile.SwimlaneSettings);
                        Assert.IsType<PredefinedFilterField>(((AttributeBasedSwimlaneSettings)agile.SwimlaneSettings).Field);
                    }
                }   
            }
        }
    }
}