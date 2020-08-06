using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class GetIssue
        {
            [Fact]
            public async Task Valid_Connection_Returns_Existing_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var client = await connection.CreateYouTrackClientAsync();

                    // Act
                    var result = await client.IssuesGetAsync(id: temporaryIssueContext.Issue.IdReadable);

                    // Assert
                    Assert.NotNull(result);
                    //Assert.Equal(temporaryIssueContext.Issue.Id, result.Id);
                    Assert.True(result.Comments.Count > 0);
                    //Assert.Equal("Bug", result.AsDynamic().Type[0]);

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}