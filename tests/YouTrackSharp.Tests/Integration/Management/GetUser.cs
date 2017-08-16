using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management
{
    public partial class UserManagementServiceTests
    {
        public class GetUser
        {
            [Fact]
            public async Task Valid_Connection_Returns_User()
            {
                // Arrange
                var connection = Connections.Demo3Token;
                var service = connection.CreateUserManagementService();
                
                // Act
                var result = await service.GetUser("demo1");
                
                // Assert
                Assert.NotNull(result);
                Assert.Equal("demo1", result.Username);
            }
        }
    }
}