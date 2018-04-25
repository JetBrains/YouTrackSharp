using System;
using System.Threading.Tasks;
using Xunit;
using YouTrackSharp.Issues;
using YouTrackSharp.Tests.Infrastructure;

namespace YouTrackSharp.Tests.Integration.Issues
{
    public partial class IssuesServiceTests
    {
        public class CreateIssue
        {
            [Fact]
            public async Task Valid_Connection_Creates_Issue()
            {
                // Arrange
                var connection = Connections.Demo1Token;
                var service = connection.CreateIssuesService();

                var newIssue = new Issue
                {
                    Summary = "Test issue - " + DateTime.UtcNow.ToString("U"),
                    Description = "This is a **test** issue created while running unit tests."
                };
                
                newIssue.SetField("Assignee", "demo1");
                newIssue.SetField("Type", "Task");
                newIssue.SetField("State", "Fixed");
                newIssue.SetField("Fix versions", new[] { "0.0.1", "0.0.2" });
                newIssue.SetField("Due Date", DateTime.UtcNow.AddDays(5));
                
                // Act
                var result = await service.CreateIssue("DP1", newIssue);
                
                // Assert
                Assert.NotNull(result);
                Assert.True(result.StartsWith("DP1"));
                
                dynamic createdIssue = await service.GetIssue(result);
                Assert.Equal(newIssue.Summary, createdIssue.Summary);
                Assert.Equal(newIssue.Description, createdIssue.Description);
                
                Assert.Equal(newIssue.GetField("Assignee").Value, createdIssue.Assignee[0].UserName);
                Assert.Equal(newIssue.GetField("Type").Value, createdIssue.Type[0]);
                Assert.Equal(newIssue.GetField("State").Value, createdIssue.State[0]);
                Assert.Equal(newIssue.GetField("Fix versions").Value, createdIssue.Fix_versions);
                Assert.Equal(newIssue.GetField("Due Date").AsDateTime().ToString("d"), createdIssue.GetField("Due Date").AsDateTime().ToString("d"));

                await service.DeleteIssue(result);
            }
        }
    }
}