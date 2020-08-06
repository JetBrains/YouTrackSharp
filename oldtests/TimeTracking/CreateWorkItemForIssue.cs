using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;
using YouTrackSharp.TimeTracking;

namespace YouTrackSharp.Tests.Integration.TimeTracking
{
    public partial class TimeTrackingServiceTests
    {
        public class CreateWorkItemForIssue
        {
            [Fact]
            public async Task Valid_Connection_Creates_Work_Items_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateTimeTrackingService();
                
                    var workTypes = await service.GetWorkTypesForProject("DP1");
                
                    // Act
                    var workItemId = await service.CreateWorkItemForIssue(temporaryIssueContext.Issue.Id, new WorkItem(DateTime.UtcNow, TimeSpan.FromMinutes(5), GetType().FullName, workTypes.First()));
                
                    // Assert
                    Assert.NotNull(workItemId);

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}