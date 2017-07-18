using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class Exists
        {
            [Theory]
            [InlineData("DP1-1", true)]
            [InlineData("DP1-0", false)]
            [InlineData("DP404-1", false)]
            public async Task Valid_Connection_Returns_Boolran_For_Issue(string issueId, bool expectedResult)
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssuesService();
                
                // Act
                var result = await service.Exists(issueId);
                
                // Assert
                Assert.Equal(expectedResult, result);
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateIssuesService();
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.Exists("NOT-EXIST"));
            }
        }
    }
}