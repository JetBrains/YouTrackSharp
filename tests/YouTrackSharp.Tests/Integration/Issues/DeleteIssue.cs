using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class DeleteIssue
        {
            [Fact]
            public async Task Valid_Connection_Deletes_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();
                
                    // Act & Assert
                    await service.DeleteIssue(temporaryIssueContext.Issue.Id);

                    await temporaryIssueContext.Destroy();
                }
            }
        }
    }
}