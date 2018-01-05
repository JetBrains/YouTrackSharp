using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

// ReSharper disable once CheckNamespace
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
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();

                    await service.ApplyCommand(temporaryIssueContext.Issue.Id, "comment", "New comment on issue");
                    await service.ApplyCommand(temporaryIssueContext.Issue.Id, "comment", "Another comment on issue");

                    // Act
                    var result = await service.GetCommentsForIssue(temporaryIssueContext.Issue.Id);

                    // Assert
                    Assert.True(result.Any());

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
                    async () => await service.GetCommentsForIssue("NOT-EXIST"));
            }
        }
    }
}