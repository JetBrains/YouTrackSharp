using System.Threading.Tasks;
using Xunit;
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
                var service = connection.CreateIssuesService();
                
                // Act
                var result = await service.GetIssue("DP1-1");
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal("DP1-1", result.Id);
                Assert.True(result.Comments.Count > 0);
                Assert.Equal("Bug", result.AsDynamic().Type[0]);
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateIssuesService();
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetIssue("NOT-EXIST"));
            }
        }
    }
}