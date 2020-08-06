using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetIssueCount
        {
            [Fact]
            public async Task Valid_Connection_Returns_Issues()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();

                    await service.ApplyCommand(temporaryIssueContext.Issue.Id, "assignee me");

                    // Act
                    var result = await service.GetIssueCount("assignee:me");
                
                    // Assert
                    Assert.True(result > 0);

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}