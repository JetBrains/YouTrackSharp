using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using YouTrackSharp.Generated;

namespace YouTrackSharp.TimeTracking
{
    /// <summary>
    /// A class that represents a REST API client for <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues-issueID-timeTracking.html">YouTrack Time Tracking User Methods</a>.
    /// It uses a <see cref="Connection" /> implementation to connect to the remote YouTrack server instance.
    /// </summary>
    public class TimeTrackingService : ITimeTrackingService
    {
        private readonly Connection _connection;
        
        /// <summary>
        /// Creates an instance of the <see cref="TimeTrackingService" /> class.
        /// </summary>
        /// <param name="connection">A <see cref="Connection" /> instance that provides a connection to the remote YouTrack server instance.</param>
        public TimeTrackingService(Connection connection)
        {
            _connection = connection ?? throw new ArgumentNullException(nameof(connection));
        }
       
        /// <inheritdoc />
        public async Task<IEnumerable<WorkType>> GetWorkTypesForProject(string projectId)
        {
            if (string.IsNullOrEmpty(projectId))
            {
                throw new ArgumentNullException(nameof(projectId));
            }

            var client = await _connection.GetAuthenticatedApiClient();
            var response =
                await client.AdminProjectsTimetrackingsettingsWorkitemtypesGetAsync(projectId, "id,name,ordinal,url", 0, -1);

            return response.Select(WorkType.FromApiEntity);
        }
       
        /// <inheritdoc />
        public async Task<IEnumerable<WorkItem>> GetWorkItemsForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedApiClient();
            var response = await client.IssuesTimetrackingWorkitemsGetAsync(
                                                        issueId, 
                                                        "id,created,updated,author(id,login),type(id,name),date,duration(id,minutes,presentation),text", 
                                                        0, -1);

            return response.Select(WorkItem.FromApiEntity);
        }
        
        /// <inheritdoc />
        public async Task<string> CreateWorkItemForIssue(string issueId, WorkItem workItem)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            var client = await _connection.GetAuthenticatedApiClient();
            var response =
                await client.IssuesTimetrackingWorkitemsPostAsync(issueId, false, "id", workItem.ToApiEntity());
            
            return response.Id;
        }
        
        /// <inheritdoc />
        public async Task UpdateWorkItemForIssue(string issueId, string workItemId, WorkItem workItem)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            if (string.IsNullOrEmpty(workItemId))
            {
                throw new ArgumentNullException(nameof(workItemId));
            }
            if (workItem == null)
            {
                throw new ArgumentNullException(nameof(workItem));
            }

            var client = await _connection.GetAuthenticatedApiClient();
            
            await client.IssuesTimetrackingWorkitemsPostAsync(issueId, workItemId, false, "id", workItem.ToApiEntity());
        }
        
        /// <inheritdoc />
        public async Task DeleteWorkItemForIssue(string issueId, string workItemId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }
            
            var client = await _connection.GetAuthenticatedApiClient();
            
            try
            {
                await client.IssuesTimetrackingWorkitemsDeleteAsync(issueId, workItemId);
            }
            catch (YouTrackErrorException e)
            {
                if (e.StatusCode == (int)HttpStatusCode.NotFound)
                {
                    return;
                }

                throw;
            }
        }
	}
}