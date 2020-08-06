using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;
using YouTrackSharp.TimeTracking;

// ReSharper disable PossibleMultipleEnumeration
namespace YouTrackSharp.Tests.Integration.TimeTracking
{
    public partial class TimeTrackingServiceTests
    {
        public class GetWorkItemsForIssue
        {
            [Fact]
            public async Task Valid_Connection_Gets_Work_Items_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateTimeTrackingService();

                    var workTypes = await service.GetWorkTypesForProject("DP1");

                    await service.CreateWorkItemForIssue(temporaryIssueContext.Issue.Id, new WorkItem(DateTime.UtcNow, TimeSpan.FromMinutes(5), GetType().FullName, workTypes.First()));
                    await service.CreateWorkItemForIssue(temporaryIssueContext.Issue.Id, new WorkItem(DateTime.UtcNow, TimeSpan.FromMinutes(10), GetType().FullName, workTypes.First()));
                    await service.CreateWorkItemForIssue(temporaryIssueContext.Issue.Id, new WorkItem(DateTime.UtcNow, TimeSpan.FromMinutes(15), GetType().FullName, workTypes.First()));

                    // Act
                    var results = await service.GetWorkItemsForIssue(temporaryIssueContext.Issue.Id);

                    // Assert
                    Assert.True(results.Any());
                    foreach (var workItem in results)
                    {
                        Assert.NotNull(workItem.Id);
                        Assert.NotNull(workItem.Date);
                        Assert.True(workItem.Duration.TotalMinutes > 0);
                        Assert.NotNull(workItem.Description);
                        Assert.NotNull(workItem.WorkType);
                        Assert.NotNull(workItem.Author);
                        Assert.NotNull(workItem.Author.Login);
                    }

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}