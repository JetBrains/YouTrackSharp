using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management
{
    public partial class UserManagementServiceTests
    {
        public class CreateAndDeleteUser
        {
            [Fact(Skip = "Don't want to pollute our server instance with random users.")]
            public async Task Valid_Connection_Creates_User()
            {
                // Arrange
                var connection = Connections.Demo3Token;
                var service = connection.CreateUserManagementService();

                var randomUsername = "test" + Guid.NewGuid().ToString().Replace("-", string.Empty);
                var randomPassword = "pwd" + Guid.NewGuid().ToString().Replace("-", string.Empty);
                
                // Act
                await service.CreateUser(randomUsername, "Test User", "test@example.org", null, randomPassword);
                
                // Assert
                try
                {
                    var result = await service.GetUser(randomUsername);
                    Assert.NotNull(result);
                    Assert.Equal(randomUsername, result.Username);
                }
                finally
                {
                    // Delete the user
                    await service.DeleteUser(randomUsername);
                }
            }
        }
    }
}