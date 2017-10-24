using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetChangesForIssue
        {
            [Fact]
            public async Task Valid_Connection_Returns_Changeset_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssuesService();
                
                // Act
                var result = await service.GetChangsetForIssue("DP1-1");
                
                // Assert
                Assert.NotNull(result);
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateIssuesService();
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetChangsetForIssue("NOT-EXIST"));
            }
        }
    }
}