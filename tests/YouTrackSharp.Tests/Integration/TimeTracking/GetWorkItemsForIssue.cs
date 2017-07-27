using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

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
                var service = connection.CreateTimeTrackingService();
                
                // Act
                var results = await service.GetWorkItemsForIssue("DP1-1");
                
                // Assert
                Assert.True(results.Any());
                foreach (var workItem in results)
                {
                    Assert.NotNull(workItem.Id);
                    Assert.NotNull(workItem.Date);
                    Assert.NotNull(workItem.Duration);
                    Assert.True(workItem.Duration.TotalMinutes > 0);
                    Assert.NotNull(workItem.Description);
                    Assert.NotNull(workItem.WorkType);
                    Assert.NotNull(workItem.Author);
                    Assert.NotNull(workItem.Author.Login);
                }
            }
        }
    }
}