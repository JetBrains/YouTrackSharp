using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetIssues
        {
            [Fact]
            public async Task Valid_Connection_Returns_Issues()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssueService();
                
                // Act
                var result = await service.GetIssues("assignee:me", take: 100);
                
                // Assert
                Assert.NotNull(result);
                foreach (dynamic issue in result)
                {
                    Assert.Equal("demo1", issue.Assignee[0].UserName);
                    Assert.NotNull(issue.ProjectShortName);
                }
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateIssueService();
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetIssuesInProject("DP1"));
            }
        }
    }
}