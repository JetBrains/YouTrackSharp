using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class ApplyCommand
        {
            [Fact]
            public async Task Valid_Connection_Applies_Command_To_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssueService();
                var comment = "Test comment via command - " + DateTime.UtcNow.ToString("U");
                
                // Act
                await service.ApplyCommand("DP1-1", "comment", comment);
                
                // Assert
                var issue = await service.GetIssue("DP1-1");
                Assert.True(issue.Comments.Count > 0);
                Assert.True(issue.Comments.Any(c => string.Equals(c.Text, comment, StringComparison.OrdinalIgnoreCase)));
            }
            
            [Fact]
            public async Task Invalid_Command_Returns_Error()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssueService();
                
                // Act
                var exception = await Assert.ThrowsAsync<YouTrackErrorException>(async () => 
                    await service.ApplyCommand("DP1-1", "gibberish"));

                // Assert
                Assert.True(exception.Message.Contains("Command [gibberish] is invalid"));
            }
        }
    }
}