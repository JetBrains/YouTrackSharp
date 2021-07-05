using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;
using YouTrackSharp.Management;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management.TimeTracking
{
	[UsedImplicitly]
    public class TimeTrackingManagementServiceTests
    {
        public class UpdateSystemWideTimeTrackingSettings
		{
			[Fact(Skip = "Ignore flaky test.")]
			public async Task Valid_Connection_Updates_Systemwide_TimeTracking_Settings()
			{
				// Arrange
				var connection = Connections.Demo3Token;
				var service = connection.CreateTimeTrackingManagementService();
				
				var random = new Random();
				var minutesADay = random.Next(1, 20);

				// Act
				var timeSettings = new SystemWideTimeTrackingSettings
				{
					MinutesADay = minutesADay,
					WorkDays = new List<int>(5)
				};
				timeSettings.WorkDays.Add(1);
				timeSettings.WorkDays.Add(2);
				timeSettings.WorkDays.Add(3);
				timeSettings.WorkDays.Add(4);
				timeSettings.WorkDays.Add(5);

				await service.UpdateSystemWideTimeTrackingSettings(timeSettings);

				// Assert
				var result = await service.GetSystemWideTimeTrackingSettings();
				Assert.NotNull(result);
				Assert.Equal(minutesADay, result.MinutesADay);
			}
		}
    }
}