using System.Threading.Tasks;
using JetBrains.Annotations;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration
{
    [UsedImplicitly]
    public partial class ConnectionTests
    {
        public class GetAuthenticatedHttpClient
        {
            [Theory]
            [MemberData(nameof(Connections.TestData.ValidConnections), MemberType = typeof(Connections.TestData))]
            public async Task Valid_Connection_Returns_Authenticated_HttpClient(Connection connection)
            {
                // Arrange & Act
                var httpClient = await connection.GetAuthenticatedHttpClient();
                var response = await httpClient.GetAsync("admin/users/me");
                
                // Assert
                Assert.True(response.IsSuccessStatusCode);
            }
            
            [Theory]
            [MemberData(nameof(Connections.TestData.InvalidConnections), MemberType = typeof(Connections.TestData))]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException(Connection connection)
            {
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await connection.GetAuthenticatedHttpClient());
            }
        }
    }
}