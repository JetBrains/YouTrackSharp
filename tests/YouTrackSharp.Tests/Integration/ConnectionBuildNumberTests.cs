using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration
{
    public partial class ConnectionTests
    {
        public class GetBuildNumber
        {
            [Theory]
            [MemberData(nameof(Connections.TestData.ValidConnections), MemberType = typeof(Connections.TestData))]
            public async Task Valid_Connection_Returns_BuildNumber(Connection connection)
            {
                // Arrange & Act
                var buildNumber = await connection.GetBuildNumber();
                
                // Assert
                Assert.True(buildNumber > 0);
            }
        }
    }
}