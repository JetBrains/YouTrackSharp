using System;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Issues;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class DeleteIssue
        {
            [Fact]
            public async Task Valid_Connection_Deletes_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssuesService();

                var testIssue = new Issue
                {
                    Summary = "Test issue - " + DateTime.UtcNow.ToString("U"),
                    Description = "This is a test issue created while running unit tests."
                };
                
                var issueId = await service.CreateIssue("DP1", testIssue);
                
                // Act & Assert
                await service.DeleteIssue(issueId);
            }
        }
    }
}