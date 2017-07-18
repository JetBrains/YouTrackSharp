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
        public class UpdateWorkItemForIssue
        {
            [Fact]
            public async Task Valid_Connection_Updates_Work_Items_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateTimeTrackingService();

                var workTypes = await service.GetWorkTypesForProject("DP1");

                var originalDescription = GetType().FullName + " " + DateTime.UtcNow.ToString("U");
                var workItem = new WorkItem(DateTime.UtcNow, TimeSpan.FromMinutes(5), originalDescription, workTypes.First());
                
                var workItemId = await service.CreateWorkItemForIssue("DP1-1", workItem);
                
                // Act & Assert
                workItem.Duration = TimeSpan.FromMinutes(1);
                workItem.Description = originalDescription + " (edited)";
                await service.UpdateWorkItemForIssue("DP1-1", workItemId, workItem);
            }
        }
    }
}