using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management
{
    public partial class UserManagementServiceTests
    {
        public class GetUsers
        {
            [Fact]
            public async Task Valid_Connection_Returns_Users()
            {
                // Arrange
                var connection = Connections.Demo3Token;
                var service = connection.CreateUserManagementService();
                
                // Act
                var result = await service.GetUsers();
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.Count > 0);
                Assert.True(result.Any(user => user.Username == "demo1"));
            }
        }
    }
}