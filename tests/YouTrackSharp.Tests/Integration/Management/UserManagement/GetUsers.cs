using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management.UserManagement
{
    [UsedImplicitly]
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
                Assert.Contains(result, user => user.Username == "demo1");
            }
        }
    }
}