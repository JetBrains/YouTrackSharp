using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.TimeTracking
{
    public partial class TimeTrackingServiceTests
    {
        public class GetWorkTypesForProject
        {
            [Fact]
            public async Task Valid_Connection_Gets_Work_Types_For_Project()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateTimeTrackingService();
                
                // Act
                var results = await service.GetWorkTypesForProject("DP1");
                
                // Assert
                Assert.True(results.Any());
                foreach (var workType in results)
                {
                    Assert.NotNull(workType.Id);
                    Assert.NotNull(workType.Name);
                    Assert.NotNull(workType.Url);
                }
            }
        }
    }
}