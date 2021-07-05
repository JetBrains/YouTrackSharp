using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management.TimeTracking
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
			    var service = connection.CreateTimeTrackingManagementService();

			    // Act
			    var results = await service.GetSystemWideTimeTrackingSettings();
			    var workdays = results.WorkDays.ToList();

				// Assert
				Assert.Equal(5, results.DaysAWeek);
			    Assert.Equal(540, results.MinutesADay);

			    Assert.Equal(1, workdays[0]);
			    Assert.Equal(2, workdays[1]);
			    Assert.Equal(3, workdays[2]);
			    Assert.Equal(4, workdays[3]);
			    Assert.Equal(5, workdays[4]);
		    }
	    }
    }
}