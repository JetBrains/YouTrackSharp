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
                var service = new UserService(connection);
                
                // Act
                var result = await service.GetCurrentUserInfo();
                
                // Assert
                Assert.NotNull(result);
                
                // TODO assert properties, too
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = new UserService(Connections.UnauthorizedConnection);
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetCurrentUserInfo());
            }
        }
    }
}