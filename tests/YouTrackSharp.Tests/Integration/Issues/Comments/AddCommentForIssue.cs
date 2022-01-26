using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class AddCommentForIssue
        {
            [Fact]
            public async Task Valid_Connection_Adds_Comment_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();
                    var commentText = "Test comment " + DateTime.UtcNow.ToString("U");

                    // Act
                    await service.AddCommentForIssue(temporaryIssueContext.Issue.Id, commentText);

                    // Assert
                    var comments = await service.GetCommentsForIssue(temporaryIssueContext.Issue.Id);

                    Assert.NotNull(comments.FirstOrDefault(comment => comment.Text.Equals(commentText)));

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}