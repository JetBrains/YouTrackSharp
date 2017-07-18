using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetCommentsForIssue
        {
            [Fact]
            public async Task Valid_Connection_Returns_Comments_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssueService();
                
                // Act
                var result = await service.GetCommentsForIssue("DP1-1");
                
                // Assert
                Assert.True(result.Any());
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateIssueService();
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetCommentsForIssue("NOT-EXIST"));
            }
        }
    }
}