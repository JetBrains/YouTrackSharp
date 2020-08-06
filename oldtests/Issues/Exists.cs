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
            [InlineData(true)]
            [InlineData(false)]
            public async Task Valid_Connection_Returns_Boolean_For_Issue(bool issueExists)
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();

                    // Act
                    var result = issueExists
                        ? await service.Exists(temporaryIssueContext.Issue.Id)
                        : await service.Exists("NOT-EXIST");

                    // Assert
                    Assert.Equal(issueExists, result);

                    await temporaryIssueContext.Destroy();
                }
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