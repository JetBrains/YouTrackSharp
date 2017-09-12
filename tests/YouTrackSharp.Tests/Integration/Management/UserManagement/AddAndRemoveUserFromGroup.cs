using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Management.UserManagement
{
    public partial class UserManagementServiceTests
    {
        public class AddAndRemoveUserFromGroup
        {
            [Fact]
            public async Task Valid_Connection_Adds_And_Removes_User_From_Group()
            {
                // Arrange
                var connection = Connections.Demo3Token;
                var service = connection.CreateUserManagementService();

                // Act
                try
                {
                    await service.AddUserToGroup("demo2", "Reporters");

                    // Assert
                    var result = await service.GetGroupsForUser("demo2");
                    Assert.NotNull(result);
                    Assert.True(result.Count > 0);
                    Assert.True(result.Any(group =>
                        string.Equals(group.Name, "Reporters", StringComparison.OrdinalIgnoreCase)));
                }
                finally
                {
                    await service.RemoveUserFromGroup("demo2", "Reporters");
                }
            }
        }
    }
}