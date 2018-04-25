using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Projects
{
    public partial class ProjectCustomFieldsServiceTests
    {
        public class GetProjectCustomField
        {
            [Fact]
            public async Task Valid_Connection_Gets_CustomFields_For_Project()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.ProjectCustomFieldsService();
                var customFieldName = "Assignee";

                // Act
                var result = await service.GetProjectCustomField("DP1", customFieldName);

                // Assert
                Assert.NotNull(result);

                Assert.Equal(customFieldName, result.Name);
                Assert.Equal(string.Empty, result.Url);
                Assert.Equal("user[1]", result.Type);
                Assert.True(result.CanBeEmpty);
                Assert.Equal("Unassigned", result.EmptyText);
            }

            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.ProjectCustomFieldsService();

                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetProjectCustomField("DP1", "Assignee"));
            }
        }
    }
}