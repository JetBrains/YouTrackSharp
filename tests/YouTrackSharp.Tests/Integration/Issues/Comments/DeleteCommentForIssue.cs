using System;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class DeleteCommentForIssue
        {
            [Fact]
            public async Task Valid_Connection_Deletes_Comment_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();
                    var commentText = "Test comment " + DateTime.UtcNow.ToString("U");
                
                    await service.ApplyCommand(temporaryIssueContext.Issue.Id, "comment", commentText);
                
                    // Act
                    var acted = false;
                    var comments = await service.GetCommentsForIssue(temporaryIssueContext.Issue.Id);                
                    foreach (var comment in comments)
                    {
                        if (string.Equals(comment.Text, commentText, StringComparison.OrdinalIgnoreCase))
                        {
                            await service.DeleteCommentForIssue(temporaryIssueContext.Issue.Id, comment.Id, permanent: true);  
                            acted = true;
                        }
                    }
                
                    // Assert
                    Assert.True(acted);

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}