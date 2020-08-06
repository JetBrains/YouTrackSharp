using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading.Tasks;

namespace YouTrackSharp.Tests.Infrastructure
{
    public class TemporaryIssueContext 
        : IDisposable
    {
        private bool _destroyed;
        
        private readonly Connection _connection;
        
        public Issue Issue { get; private set; }
        
        public static async Task<TemporaryIssueContext> Create(Connection connection, Type testType)
        {
            return await Create(connection, "Temporary issue " + DateTime.UtcNow.ToString(CultureInfo.InvariantCulture) + " by " + testType.FullName, "This is a temporary issue created by " + testType.FullName + ".");
        }

        // ReSharper disable once MemberCanBePrivate.Global
        public static async Task<TemporaryIssueContext> Create(Connection connection, string summary, string description)
        {
            var temporaryIssueContext = new TemporaryIssueContext(connection, summary, description);
            
            temporaryIssueContext.Issue = await CreateTemporaryIssue(temporaryIssueContext._connection, temporaryIssueContext.Issue);
            
            return temporaryIssueContext;
        }

        public async Task Destroy()
        {
            if (!string.IsNullOrEmpty(Issue?.IdReadable) && !_destroyed)
            {
                await DeleteTemporaryIssue(_connection, Issue.IdReadable);
                
                _destroyed = true;
                Issue = null;
            }
        }
        
        private TemporaryIssueContext(Connection connection, string summary, string description)
        {
            _connection = connection;
            
            Issue = new Issue
            {
                Summary = summary,
                Description = description
            };

            // Issue.CustomFields = new List<IssueCustomField>
            // {
            //     new IssueCustomField() {  }
            // };
            // TODO MIGRATION Issue.("State", "Fixed");
        }
        
        private static async Task<Issue> CreateTemporaryIssue(Connection connection, Issue issue)
        {
            var client = await connection.CreateYouTrackProjectsClientAsync();

            return await client.IssuesPostAsync(id: "DP1", body: issue);
        }
        
        private static async Task DeleteTemporaryIssue(Connection connection, string issueId)
        {
            var client = await connection.CreateYouTrackProjectsClientAsync();

            await client.IssuesDeleteAsync(id: "DP1", issueId: issueId);
        }

        public void Dispose()
        {
            if (!string.IsNullOrEmpty(Issue?.IdReadable) && !_destroyed)
            {
                // ReSharper disable once LocalizableElement
                Console.WriteLine($"The temporary issue will not be cleaned up. Please call the {nameof(TemporaryIssueContext)}.{nameof(Destroy)}() method before disposing.");
            }
        }
    }
}