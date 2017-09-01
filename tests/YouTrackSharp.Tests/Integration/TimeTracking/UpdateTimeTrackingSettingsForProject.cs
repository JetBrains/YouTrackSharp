using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;
using YouTrackSharp.TimeTracking;

namespace YouTrackSharp.Tests.Integration.TimeTracking
{
    public partial class TimeTrackingServiceTests
    {
        public class UpdateTimeTrackingSettingsForProject
		{
            [Fact]
            public async Task Valid_Connection_Update_TimeTracking_Settings_For_Project()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateTimeTrackingService();

				// Act & Assert
				await service.UpdateTimeTrackingSettingsForProject("DP1", new TimeTrackingSettings {Enabled = false});
            }
        }
    }
}