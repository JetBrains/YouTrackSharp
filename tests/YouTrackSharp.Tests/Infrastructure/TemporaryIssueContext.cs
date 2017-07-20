using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using YouTrackSharp.Issues;

namespace YouTrackSharp.Tests.Infrastructure
{
    public class TemporaryIssueContext 
        : IDisposable
    {
        private readonly Connection _connection;
        
        public Issue Issue { get; private set; }

        
        public static async Task<TemporaryIssueContext> Create(Connection connection)
        {
            return await Create(connection, "Temporary issue " + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture), "This is a temporary issue.");
        }
        
        public static async Task<TemporaryIssueContext> Create(Connection connection, Type testType)
        {
            return await Create(connection, "Temporary issue " + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) + " by " + testType.FullName, "This is a temporary issue created by " + testType.FullName + ".");
        }

        public static async Task<TemporaryIssueContext> Create(Connection connection, string summary, string description)
        {
            var temporaryIssueContext = new TemporaryIssueContext(connection, summary, description);
            
            temporaryIssueContext.Issue = await CreateTemporaryIssue(temporaryIssueContext._connection, temporaryIssueContext.Issue);
            
            return temporaryIssueContext;
        }
        
        private TemporaryIssueContext(Connection connection, string summary, string description)
        {
            _connection = connection;
            
            Issue = new Issue
            {
                Summary = summary,
                Description = description
            };

            Issue.SetField("State", "Fixed");
        }
        
        private static async Task<Issue> CreateTemporaryIssue(Connection connection, Issue issue)
        {
            var service = connection.CreateIssuesService();

            var issueId = await service.CreateIssue("DP1", issue);

            return await service.GetIssue(issueId);
        }
        
        private static async Task DeleteTemporaryIssue(Connection connection, string issueId)
        {
            var service = connection.CreateIssuesService();

            await service.DeleteIssue(issueId);
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(Issue?.Id))
            {
                DeleteTemporaryIssue(_connection, Issue.Id)
                    .GetAwaiter()
                    .GetResult();
            }
        }
    }
}