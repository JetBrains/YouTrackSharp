using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Projects;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Projects
{
    public class ProjectsServiceTests
    {
        public class GetAccessibleProjects
        {
            [Fact]
            public async Task Valid_Connection_Returns_Accessible_Projects()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateProjectsService();
                
                // Act
                var result = await service.GetAccessibleProjects();
                
                // Assert
                Assert.NotNull(result);

                var demoProject = result.FirstOrDefault(p => p.ShortName == "DP1");
                Assert.NotNull(demoProject);
                Assert.Equal("DP1", demoProject.ShortName);
                Assert.Equal("DemoProject1", demoProject.Name);
                Assert.Null(demoProject.Description);
            }
            
            [Fact]
            public async Task Valid_Connection_Returns_Accessible_Projects_Verbose()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateProjectsService();
                
                // Act
                var result = await service.GetAccessibleProjects(verbose: true);
                
                // Assert
                Assert.NotNull(result);

                var demoProject = result.FirstOrDefault(p => p.ShortName == "DP1");
                Assert.NotNull(demoProject);
                Assert.Equal("DP1", demoProject.ShortName);
                Assert.Equal("DemoProject1", demoProject.Name);
                Assert.Equal("Demo project 1", demoProject.Description);
                Assert.True(demoProject.Versions.Any(v => v == "0.0.1"));
                Assert.True(demoProject.AssigneesLogin.Any(a => a.Value == "demo1"));
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateProjectsService();
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetAccessibleProjects());
            }
        }
    }
}