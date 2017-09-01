using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;
using YouTrackSharp.TimeTracking;

namespace YouTrackSharp.Tests.Integration.TimeTracking
{
    public partial class TimeTrackingServiceTests
    {
        public class UpdateSystemwideTimeTrackingSettings
		{
			[Fact]
			public async Task Valid_Connection_Update_Systemwide_TimeTracking_Settings()
			{
				// Arrange
				var connection = Connections.Demo3Token;
				var service = connection.CreateTimeTrackingService();

				// Act & Assert
				var timeSettings = new SystemWideTimeTrackingSettings
				{
					HoursADay = 8,
					WorkDays = new List<SubValue<int>>(5)
				};
				timeSettings.WorkDays.Add(new SubValue<int> {Value = 1});
				timeSettings.WorkDays.Add(new SubValue<int> {Value = 2});
				timeSettings.WorkDays.Add(new SubValue<int> {Value = 3});
				timeSettings.WorkDays.Add(new SubValue<int> {Value = 4});
				timeSettings.WorkDays.Add(new SubValue<int> {Value = 5});

				await service.UpdateSystemWideTimeTrackingSettings(timeSettings);
			}
		}
    }
}