using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Projects;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Projects
{
    public partial class ProjectCustomFieldsServiceTests
    {
        public class CreateProjectCustomField
        {
            [Fact]
            public async Task Valid_Connection_Creates_CustomField_For_Project()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.ProjectCustomFieldsService();
                var customField = new CustomField { Name = "TestField" };
                var projectId = "DP1";

                try
                {
                    // Act
                    await service.CreateProjectCustomField(projectId, customField);

                    var created = await service.GetProjectCustomField(projectId, customField.Name);

                    // Assert
                    Assert.NotNull(created);

                    Assert.Equal(customField.Name, created.Name);
                    Assert.Equal(customField.EmptyText, string.Empty);
                }
                finally
                {
                    // Cleanup
                    await service.DeleteProjectCustomField(projectId, customField.Name);
                }
            }

            [Fact]
            public async Task Valid_Connection_Creates_CustomField_With_EmptyText_For_Project()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.ProjectCustomFieldsService();
                var customField = new CustomField { Name = "TestField", EmptyText = "empty" };
                var projectId = "DP1";

                try
                {
                    // Act
                    await service.CreateProjectCustomField(projectId, customField);

                    var created = await service.GetProjectCustomField(projectId, customField.Name);

                    // Assert
                    Assert.NotNull(created);

                    Assert.Equal(customField.Name, created.Name);
                    Assert.Equal(customField.EmptyText, created.EmptyText);
                }
                finally
                {
                    // Cleanup
                    await service.DeleteProjectCustomField(projectId, customField.Name);
                }
            }

            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.ProjectCustomFieldsService();

                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetProjectCustomField("DP1", "TestField"));
            }
        }
    }
}