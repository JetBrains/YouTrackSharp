using System;
using System.Linq;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;
using YouTrackSharp.TimeTracking;

namespace YouTrackSharp.Tests.Integration.TimeTracking
{
    [UsedImplicitly]
    public partial class TimeTrackingServiceTests
    {
        public class UpdateWorkItemForIssue
        {
            [Fact]
            public async Task Valid_Connection_Updates_Work_Items_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateTimeTrackingService();
                
                    var workTypes = await service.GetWorkTypesForProject("DP1");

                    var originalDescription = GetType().FullName + " " + DateTime.UtcNow.ToString("U");
                    var workItem = new WorkItem(DateTime.UtcNow, TimeSpan.FromMinutes(5), originalDescription, workTypes.First());
                
                    var workItemId = await service.CreateWorkItemForIssue(temporaryIssueContext.Issue.Id, workItem);
                
                    // Act & Assert
                    workItem.Duration = TimeSpan.FromMinutes(1);
                    workItem.Description = originalDescription + " (edited)";
                    await service.UpdateWorkItemForIssue(temporaryIssueContext.Issue.Id, workItemId, workItem);
                    
                    var results = await service.GetWorkItemsForIssue(temporaryIssueContext.Issue.Id);

                    var apiWorkItem = results.SingleOrDefault(w => w.Id == workItemId);
                    
                    Assert.NotNull(apiWorkItem);
                    Assert.NotNull(apiWorkItem.Updated);

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}