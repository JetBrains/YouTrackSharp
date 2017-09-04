using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Management;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management.TimeTracking
{
    public partial class TimeTrackingServiceTests
    {
        public class TimeTrackingManagementServiceTests
		{
            [Fact]
            public async Task Valid_Connection_Updates_TimeTracking_Settings_For_Project()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateTimeTrackingManagementService();

				// Act
                await service.UpdateTimeTrackingSettingsForProject("DP1", new TimeTrackingSettings { Enabled = true });

                // Assert
                var result = await service.GetTimeTrackingSettingsForProject("DP1");
                Assert.True(result.Enabled);
            }
        }
    }
}