using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.TimeTracking
{
    public partial class TimeTrackingServiceTests
    {
	    public class GetSystemwideTimeTrackingSettings
	    {
		    [Fact]
		    public async Task Valid_Connection_Gets_Systemwide_TimeTracking_Settings()
		    {
			    // Arrange
			    var connection = Connections.Demo3Token;
			    var service = connection.CreateTimeTrackingService();

			    // Act
			    var results = await service.GetSystemWideTimeTrackingSettings();
			    var workdays = results.WorkDays.ToList();

				// Assert
				Assert.Equal(5, results.DaysAWeek);
			    Assert.Equal(8, results.HoursADay);

			    Assert.Equal(1, workdays[0].Value);
			    Assert.Equal(2, workdays[1].Value);
			    Assert.Equal(3, workdays[2].Value);
			    Assert.Equal(4, workdays[3].Value);
			    Assert.Equal(5, workdays[4].Value);
		    }
	    }
    }
}