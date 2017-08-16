using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management
{
    public partial class UserManagementServiceTests
    {
        public class CreateAndUpdateUser
        {
            [Fact(Skip = "Don't want to pollute our server instance with random users.")]
            public async Task Valid_Connection_Updates_User()
            {
                // Arrange
                var connection = Connections.Demo3Token;
                var service = connection.CreateUserManagementService();

                var randomUsername = "test" + Guid.NewGuid().ToString().Replace("-", string.Empty);
                var randomPassword = "pwd" + Guid.NewGuid().ToString().Replace("-", string.Empty);
                
                await service.CreateUser(randomUsername, "Test User", "test1@example.org", null, randomPassword);
                
                // Act
                await service.UpdateUser(randomUsername, "Test user (updated)");
                
                // Assert
                try
                {
                    var result = await service.GetUser(randomUsername);
                    
                    Assert.NotNull(result);
                    Assert.Equal(randomUsername, "Test user (updated)");
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