using System;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Issues;
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
                var service = connection.CreateIssuesService();
                var testIssue = new Issue
                {
                    Summary = "Test issue - " + DateTime.UtcNow.ToString("U"),
                    Description = "This is a test issue created while running unit tests."
                };
                
                testIssue.SetField("State", "Fixed");
                
                var testIssueId = await service.CreateIssue("DP1", testIssue);
                
                // Act
                await service.UpdateIssue(testIssueId, testIssue.Summary + " (updated)", testIssue.Description + " (updated)");
                
                // Assert
                var updatedIssue = await service.GetIssue(testIssueId);
                Assert.Equal(testIssue.Summary + " (updated)", updatedIssue.Summary);
                Assert.Equal(testIssue.Description + " (updated)", updatedIssue.Description);
            }
        }
    }
}