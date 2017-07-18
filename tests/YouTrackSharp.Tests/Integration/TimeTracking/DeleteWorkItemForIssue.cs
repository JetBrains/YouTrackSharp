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
        public class DeleteWorkItemForIssue
        {
            [Fact]
            public async Task Valid_Connection_Deletes_Work_Items_For_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateTimeTrackingService();

                var workTypes = await service.GetWorkTypesForProject("DP1");

                var workItem = new WorkItem(DateTime.UtcNow, TimeSpan.FromMinutes(5), "Run unit tests. " + DateTime.UtcNow.ToString("U"), workTypes.First());
                
                var workItemId = await service.CreateWorkItemForIssue("DP1-1", workItem);
                
                // Act & Assert
                await service.DeleteWorkItemForIssue("DP1-1", workItemId);
            }
        }
    }
}