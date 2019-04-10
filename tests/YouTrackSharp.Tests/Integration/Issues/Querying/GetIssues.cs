using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetIssues
        {
            [Fact]
            public async Task Valid_Connection_Returns_Issues()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();

                    await service.ApplyCommand(temporaryIssueContext.Issue.Id, "assignee me");

                    // Act
                    var result = await service.GetIssues("assignee:me", take: 100);

                    // Assert
                    Assert.NotNull(result);
                    foreach (dynamic issue in result)
                    {
                        Assert.Equal("demo1", issue.Assignee[0].UserName);
                        Assert.NotNull(issue.ProjectShortName);
                    }

                    await temporaryIssueContext.Destroy();
                }
            }

            [Fact]
            public async Task Valid_Connection_Page_Issues()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();
                    
                    await service.ApplyCommand(temporaryIssueContext.Issue.Id, "assignee me");

                    // Act
                    var totalResultsCount = 0;
                    
                    var skip = 0;
                    while (skip < 1000)
                    {
                        var result = await service.GetIssues("assignee: me", skip, take: 100);
                        if (result?.Count > 0)
                        {
                            totalResultsCount += result.Count;
                        }
                        else
                        {
                            break;
                        }

                        skip += 100;
                    }
                    
                    // Assert
                    Assert.True(totalResultsCount > 100);

                    await temporaryIssueContext.Destroy();
                }
            }

            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateIssuesService();
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetIssuesInProject("DP1"));
            }
        }
    }
}