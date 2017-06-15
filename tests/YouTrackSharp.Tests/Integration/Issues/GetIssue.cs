using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Issues;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetIssue
        {
            [Fact]
            public async Task Valid_Connection_Returns_Existing_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = new IssuesService(connection);
                
                // Act
                var result = await service.GetIssue("DP1-1");
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal("DP1-1", result.Id);
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = new IssuesService(Connections.UnauthorizedConnection);
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetIssue("NOT-EXIST"));
            }
        }
    }
}