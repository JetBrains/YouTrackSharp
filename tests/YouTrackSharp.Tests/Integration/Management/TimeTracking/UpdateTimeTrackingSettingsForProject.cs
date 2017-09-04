using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Management;
using YouTrackSharp.Tests.Infrastructure;
using YouTrackSharp.TimeTracking;

namespace YouTrackSharp.Tests.Integration.TimeTracking
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
                try
                {
                    await service.UpdateTimeTrackingSettingsForProject("DP1", new TimeTrackingSettings { Enabled = false });

                    // Assert
                    var result = await service.GetTimeTrackingSettingsForProject("DP1");
                    Assert.False(result.Enabled);
                }
                finally
                {
                    // Restore to original
                    await service.UpdateTimeTrackingSettingsForProject("DP1", new TimeTrackingSettings { Enabled = true });
                    
                    var result = await service.GetTimeTrackingSettingsForProject("DP1");
                    Assert.True(result.Enabled);
                }
            }
        }
    }
}