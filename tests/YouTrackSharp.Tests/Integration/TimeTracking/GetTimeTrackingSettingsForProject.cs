using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.TimeTracking
{
    public partial class TimeTrackingServiceTests
    {
        public class GetTimeTrackingSettingsForProject
		{
            [Fact]
            public async Task Valid_Connection_Gets_TimeTracking_Settings_For_Project()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateTimeTrackingService();
                
                // Act
                var results = await service.GetTimeTrackingSettingsForProject("DP1");
                
                // Assert
                Assert.True(results.Enabled);
                Assert.Equal("Estimation", results.Estimation.Name);
                Assert.Equal("Spent time", results.SpentTime.Name);
            }
        }
    }
}