using System;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

// ReSharper disable once CheckNamespace
namespace YouTrackSharp.Tests.Integration.Issues
{
    [UsedImplicitly]
    public partial class IssuesServiceTests
    {
        public class GetIssuesInProject
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
                    var result = await service.GetIssuesInProject("DP1", filter: "assignee:me");

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
            public async Task Valid_Connection_Returns_Updated_Issues()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();

                    var updated = DateTime.Now - TimeSpan.FromMinutes(1);
                    
                    await service.ApplyCommand(temporaryIssueContext.Issue.Id, "assignee me");

                    // Act
                    var result = await service.GetIssuesInProject("DP1", "assignee: me", 0, take: 10, updated);

                    // Assert
                    Assert.True(result.Count > 0);

                    await temporaryIssueContext.Destroy();
                }
            }
            
            [Fact]
            public async Task Valid_Connection_Not_Returns_Issues_From_Future()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();

                    await service.ApplyCommand(temporaryIssueContext.Issue.Id, "assignee me");

                    // Act
                    var result = await service.GetIssuesInProject("DP1", "assignee: me", 0, take: 1,
                        DateTime.Now + TimeSpan.FromMinutes(2));

                    // Assert
                    Assert.Equal(0, result.Count);

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