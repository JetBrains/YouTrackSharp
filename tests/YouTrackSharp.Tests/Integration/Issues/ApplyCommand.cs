using System;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Generated;
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
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();
                    var commentText = "Test comment via command - " + DateTime.UtcNow.ToString("U");
                
                    // Act
                    await service.ApplyCommand(temporaryIssueContext.Issue.Id, "comment", commentText);
                
                    // Assert
                    var issue = await service.GetIssue(temporaryIssueContext.Issue.Id);
                    Assert.True(issue.Comments.Count > 0);
                    Assert.Contains(issue.Comments, c => string.Equals(c.Text, commentText, StringComparison.OrdinalIgnoreCase));

                    await temporaryIssueContext.Destroy();
                }
            }
            
            [Fact]
            public async Task Invalid_Command_Returns_Error()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssuesService();

                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    // Act
                    var exception = await Assert.ThrowsAsync<YouTrackErrorException>(async () =>
                        await service.ApplyCommand(temporaryIssueContext.Issue.Id, "gibberish"));

                    // Assert
                    Assert.Contains("Unknown command: gibberish", exception.Message);

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}