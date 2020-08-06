using System;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management.UserManagement
{
    public partial class UserManagementServiceTests
    {
        public class GetGroupsForUser
        {
            [Fact]
            public async Task Valid_Connection_Returns_Groups_ForUser()
            {
                // Arrange
                var connection = Connections.Demo3Token;
                var service = connection.CreateUserManagementService();

                // Act
                var result = await service.GetGroupsForUser("demo3");

                // Assert
                Assert.NotNull(result);
                Assert.True(result.Count > 0);
                Assert.Contains(result, group => 
                    string.Equals(group.Name, "All Users", StringComparison.OrdinalIgnoreCase));
            }
        }
    }
}