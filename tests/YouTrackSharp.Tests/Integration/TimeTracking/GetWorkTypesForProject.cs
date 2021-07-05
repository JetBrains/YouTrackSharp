using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;
using YouTrackSharp.TimeTracking;

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
                var workTypes = results as WorkType[] ?? results.ToArray();
                Assert.True(workTypes.Any());
                foreach (var workType in workTypes)
                {
                    Assert.NotNull(workType.Id);
                    Assert.NotNull(workType.Name);
                }
            }
        }
    }
}