using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Issues;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetAttachmentsForIssue
        {
            [Fact]
            public async Task Valid_Connection_Gets_Attachments_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssueService();
                
                var issue = new Issue
                {
                    Summary = "Test issue - " + DateTime.UtcNow.ToString("U"),
                    Description = "This is a test issue created while running unit tests."
                };
                
                issue.SetField("State", "Fixed");
                
                var issueId = await service.CreateIssue("DP1", issue);

                for (var i = 1; i <= 3; i++)
                {
                    using (var attachmentStream = await TestUtilities.GenerateAttachmentStream("Generated by unit test."))
                    {
                        await service.AttachFileToIssue(issueId, $"file-{i}.txt", attachmentStream);
                    }
                }
                
                
                // Act
                var attachments = await service.GetAttachmentsForIssue(issueId);
                
                // Assert
                Assert.True(attachments.Count() == 3);
            }
        }
    }
}