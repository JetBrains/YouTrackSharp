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
                    var service = connection.CreateIssuesService();

                    await service.ApplyCommand(temporaryIssueContext.Issue.Id, "comment", "This is a test comment.");

                    // Act
                    var result = await service.GetIssue(temporaryIssueContext.Issue.Id);

                    // Assert
                    Assert.NotNull(result);
                    Assert.Equal(temporaryIssueContext.Issue.Id, result.Id);
                    Assert.True(result.Comments.Count > 0);

                    await temporaryIssueContext.Destroy();
                }
            }

            [Fact]
            public async Task Valid_Connection_Returns_Correct_Issue_Fields()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                using (var temporaryIssueContext = await TemporaryIssueContext.Create(connection, GetType()))
                {
                    var service = connection.CreateIssuesService();

                    await service.ApplyCommand(temporaryIssueContext.Issue.Id,
                        "Priority Major Type Exception State Open StateMachineField in progress" +
                        " Due date 2023-03-01 Assignee root Subsystem Subsystem1 Fix versions 0.0.1" +
                        " Fixed in build 2023.1.1000 Estimation 1w StringField \"test\" IntField 1 FloatField 1.1");

                    // Act
                    var result = await service.GetIssue(temporaryIssueContext.Issue.Id);

                    // Assert
                    Assert.NotNull(result);
                    Assert.Equal(temporaryIssueContext.Issue.Id, result.Id);
                    Assert.Equal("Major", result.AsDynamic().Priority[0]);
                    Assert.Equal("Exception", result.AsDynamic().Type[0]);
                    Assert.Equal("Open", result.AsDynamic().State[0]);
                    Assert.Equal("In progress", result.AsDynamic().StateMachineField[0]);
                    Assert.Equal("1677672000000", result.AsDynamic().Due_Date[0]);
                    Assert.Equal("root", result.AsDynamic().Assignee[0].UserName);
                    Assert.Equal("Subsystem1", result.AsDynamic().Subsystem[0]);
                    Assert.Equal("0.0.1", result.AsDynamic().Fix_versions[0]);
                    Assert.Equal("2023.1.1000", result.AsDynamic().Fixed_in_build[0]);
                    Assert.Equal("2700", result.AsDynamic().Estimation[0]);
                    Assert.Equal("Exception", result.AsDynamic().Type[0]);
                    Assert.Equal("test", result.AsDynamic().StringField[0]);
                    Assert.Equal("1", result.AsDynamic().IntField[0]);
                    Assert.Equal("1.1", result.AsDynamic().FloatField[0]);

                    await temporaryIssueContext.Destroy();
                }
            }
            
            [Fact]
            public async Task Invalid_Connection_Throws_UnauthorizedConnectionException()
            {
                // Arrange
                var service = Connections.UnauthorizedConnection.CreateIssuesService();
                
                // Act & Assert
                await Assert.ThrowsAsync<UnauthorizedConnectionException>(
                    async () => await service.GetIssue("NOT-EXIST"));
            }
        }
    }
}