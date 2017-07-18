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
                bool acted = false;
                
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssueService();

                var commentText = "Test comment " + DateTime.UtcNow.ToString("U");
                await service.ApplyCommand("DP1-1", "comment", commentText);
                
                var comments = await service.GetCommentsForIssue("DP1-1");                
                foreach (var comment in comments)
                {
                    if (string.Equals(comment.Text, commentText, StringComparison.OrdinalIgnoreCase))
                    {
                        // Act & Assert
                        await service.DeleteCommentForIssue("DP1-1", comment.Id, permanent: true);  
                        acted = true;
                    }
                }
                
                Assert.True(acted);
            }
        }
    }
}