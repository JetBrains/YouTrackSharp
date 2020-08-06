using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetLinksForIssue
        {
            [Fact]
            public async Task Valid_Connection_Returns_Issue_Links()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext1 = await TemporaryIssueContext.Create(connection, GetType()))
                using (var temporaryIssueContext2 = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();

                    await service.ApplyCommand(temporaryIssueContext1.Issue.Id, "assignee me");
                    await service.ApplyCommand(temporaryIssueContext2.Issue.Id, "assignee me relates to " + temporaryIssueContext1.Issue.Id);

                    // Act
                    var result = await service.GetLinksForIssue(temporaryIssueContext1.Issue.Id);

                    // Assert
                    Assert.NotNull(result);
                    foreach (var link in result)
                    {
                        Assert.NotNull(link.InwardType);
                        Assert.NotNull(link.OutwardType);
                        Assert.NotNull(link.TypeName);
                        Assert.NotNull(link.Source);
                        Assert.NotNull(link.Target);
                    }

                    await temporaryIssueContext2.Destroy();
                    await temporaryIssueContext1.Destroy();
                }
            }
        }
    }
}