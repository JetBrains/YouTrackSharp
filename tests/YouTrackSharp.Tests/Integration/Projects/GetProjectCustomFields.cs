using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Projects
{
    [UsedImplicitly]
    public partial class ProjectCustomFieldsServiceTests
    {
        public class GetProjectCustomFields
        {
            [Fact]
            public async Task Valid_Connection_Gets_CustomFields_For_Project()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.ProjectCustomFieldsService();

                // Act
                var result = await service.GetProjectCustomFields("DP1");

                // Assert
                Assert.NotEmpty(result);
                foreach (var customField in result)
                {
                    Assert.NotNull(customField);
                    Assert.NotEqual(string.Empty, customField.Name);
                }
            }

            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.ProjectCustomFieldsService();

                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetProjectCustomFields("DP1"));
            }
        }
    }
}