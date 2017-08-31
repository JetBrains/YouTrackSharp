using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management
{
    public partial class UserManagementServiceTests
    {
        public class CreateAndMergeUser
        {
            [Fact(Skip = "Don't want to pollute our server instance with random users.")]
            public async Task Valid_Connection_Merges_User()
            {
                // Arrange
                var connection = Connections.Demo3Token;
                var service = connection.CreateUserManagementService();

                var randomUsername = "test" + Guid.NewGuid().ToString().Replace("-", string.Empty);
                var randomPassword = "pwd" + Guid.NewGuid().ToString().Replace("-", string.Empty);
                
                await service.CreateUser(randomUsername + "1", "Test 1 User", "test1@example.org", null, randomPassword);
                await service.CreateUser(randomUsername + "2", "Test 2 User", "test2@example.org", null, randomPassword);
                
                // Act
                await service.MergeUsers(randomUsername + "1", randomUsername + "2");
                
                // Assert
                try
                {
                    var result1 = await service.GetUser(randomUsername + "1");
                    var result2 = await service.GetUser(randomUsername + "2");
                    
                    Assert.Null(result1);
                    
                    Assert.NotNull(result2);
                    Assert.Equal(randomUsername + "2", result2.Username);
                }
                finally
                {
                    // Delete the user
                    await service.DeleteUser(randomUsername + "2");
                }
            }
        }
    }
}