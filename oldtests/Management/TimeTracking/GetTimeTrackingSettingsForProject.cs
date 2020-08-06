using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management.TimeTracking
{
    public partial class TimeTrackingServiceTests
    {
        public class GetTimeTrackingSettingsForProject
		{
            [Fact]
            public async Task Valid_Connection_Gets_TimeTracking_Settings_For_Project()
            {
                // Arrange
                var connection = Connections.Demo3Token;
                var service = connection.CreateTimeTrackingManagementService();
                
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