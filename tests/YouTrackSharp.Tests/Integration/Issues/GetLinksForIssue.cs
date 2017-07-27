using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetLinksForIssue
        {
            [Fact]
            public async Task Valid_Connection_Returns_Issue_Links()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssuesService();
                
                // Act
                var result = await service.GetLinksForIssue("DP1-1");
                
                // Assert
                Assert.NotNull(result);
                foreach (var link in result)
                {
                    Assert.NotNull(link.InwardType);
                    Assert.NotNull(link.OutwardType);
                    Assert.NotNull(link.TypeName);
                    Assert.NotNull(link.Source);
                    Assert.NotNull(link.Target);
                }
            }
        }
    }
}