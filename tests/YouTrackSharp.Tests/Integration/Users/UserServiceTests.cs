using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;
using YouTrackSharp.Users;

namespace YouTrackSharp.Tests.Integration.Users
{
    public class UserServiceTests
    {
        public class GetCurrentUserInfo
        {
            [Theory]
            [MemberData(nameof(Connections.TestData.ValidConnections), MemberType = typeof(Connections.TestData))]
            public async Task Valid_Connection_Returns_Current_User(Connection connection)
            {
                // Arrange
                var userService = new UserService(connection);
                
                // Act
                var result = await userService.GetCurrentUserInfo();
                
                // Assert
                Assert.NotNull(result);
                
                // TODO assert properties, too
            }
            
            [Theory]
            [MemberData(nameof(Connections.TestData.InvalidConnections), MemberType = typeof(Connections.TestData))]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException(Connection connection)
            {
                // Arrange
                var userService = new UserService(connection);
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await userService.GetCurrentUserInfo());
            }
        }
    }
}