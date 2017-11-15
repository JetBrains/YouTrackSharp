using System;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Projects
{
    public partial class ProjectCustomFieldsServiceTests
    {
        public class DeleteProjectCustomField
        {
            [Fact]
            public async Task Valid_Connection_Deletes_CustomField_For_Project()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.ProjectCustomFieldsService();

                // Act & Assert
                var acted = false;
                await service.DeleteProjectCustomField("DP1", "TestField");
                acted = true;

                Assert.True(acted);
            }

            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.ProjectCustomFieldsService();

                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.DeleteProjectCustomField("DP1", "TestField"));
            }
        }
    }
}