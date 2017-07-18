using System;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Issues;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class CreateIssue
        {
            [Fact]
            public async Task Valid_Connection_Creates_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssueService();
                
                var newIssue = new Issue();
                newIssue.Summary = "Test issue - " + DateTime.UtcNow.ToString("U");
                newIssue.Description = "This is a test issue created while running unit tests.";
                
                // Act
                var result = await service.CreateIssue("DP1", newIssue);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.StartsWith("DP1"));
                
                var createdIssue = await service.GetIssue(result);
                Assert.Equal(newIssue.Summary, createdIssue.Summary);
                Assert.Equal(newIssue.Description, createdIssue.Description);
            }
        }
    }
}