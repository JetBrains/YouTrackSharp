using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Issues;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetIssuesInProject
        {
            [Fact]
            public async Task Valid_Connection_Returns_Issues()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = new IssuesService(connection);
                
                // Act
                var result = await service.GetIssuesInProject("DP1", filter: "assignee:me");
                
                // Assert
                Assert.NotNull(result);
                Assert.Collection(result, issue => 
                    Assert.Equal("DP1", ((dynamic)issue).ProjectShortName));
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = new IssuesService(Connections.UnauthorizedConnection);
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetIssuesInProject("DP1"));
            }
        }
    }
}