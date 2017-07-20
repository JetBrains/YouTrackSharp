using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class UpdateIssue
        {
            [Fact]
            public async Task Valid_Connection_Updates_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();
                
                    // Act
                    await service.UpdateIssue(temporaryIssueContext.Issue.Id, temporaryIssueContext.Issue.Summary + " (updated)", temporaryIssueContext.Issue.Description + " (updated)");
                
                    // Assert
                    var updatedIssue = await service.GetIssue(temporaryIssueContext.Issue.Id);
                    Assert.Equal(temporaryIssueContext.Issue.Summary + " (updated)", updatedIssue.Summary);
                    Assert.Equal(temporaryIssueContext.Issue.Description + " (updated)", updatedIssue.Description);

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}