using System.Collections.Generic;
using System.Threading.Tasks;

namespace YouTrackSharp.TimeTracking
{
    public interface ITimeTrackingService
    {
        /// <summary>
        /// Get work types for a specific project from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-admin-projects-projectID-timeTrackingSettings-workItemTypes.html#get_all-WorkItemType-method">GET Work Types for a Project</a>.</remarks>
        /// <param name="projectId">Id of the project to get work items for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="WorkType" /> for the requested project <paramref name="projectId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="projectId"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance returned an HTTP 400 status code, usually indicating time tracking is not enabled.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<IEnumerable<WorkType>> GetWorkTypesForProject(string projectId);

        /// <summary>
        /// Get work items for a specific issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues-issueID-timeTracking-workItems.html#get_all-IssueWorkItem-method">Get Available Work Items of Issue</a>.</remarks>
        /// <param name="issueId">Id of the issue to get work items for.</param>
        /// <returns>An <see cref="T:System.Collections.Generic.IEnumerable`1" /> of <see cref="WorkItem" /> for the requested issue <paramref name="issueId"/>.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance returned an HTTP 400 status code, usually indicating time tracking is not enabled.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<IEnumerable<WorkItem>> GetWorkItemsForIssue(string issueId);

        /// <summary>
        /// Creates a work item for an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/resource-api-issues-issueID-timeTracking-workItems.html#create-IssueWorkItem-method">Create New Work Item</a>.</remarks>
        /// <param name="issueId">Id of the issue to create the work item for.</param>
        /// <param name="workItem">The <see cref="WorkItem"/> to create.</param>
        /// <returns>The newly created <see cref="WorkItem" />'s id on the server.</returns>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="workItem"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task<string> CreateWorkItemForIssue(string issueId, WorkItem workItem);

        /// <summary>
        /// Updates a work item for an issue on the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-issues-issueID-timeTracking-workItems.html#update-IssueWorkItem-method">Edit Existing Work Item</a>.</remarks>
        /// <param name="issueId">Id of the issue to update the work item for.</param>
        /// <param name="workItemId">Id of the work item to update.</param>
        /// <param name="workItem">The <see cref="WorkItem"/> to update.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/>, <paramref name="workItemId"/> or <paramref name="workItem"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance failed and YouTrack reported an error message.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task UpdateWorkItemForIssue(string issueId, string workItemId, WorkItem workItem);

        /// <summary>
        /// Deletes a work item for an issue from the server.
        /// </summary>
        /// <remarks>Uses the REST API <a href="https://www.jetbrains.com/help/youtrack/devportal/operations-api-issues-issueID-timeTracking-workItems.html#update-IssueWorkItem-method">Delete Existing Work Item</a>.</remarks>
        /// <param name="issueId">Id of the issue to delete the work item for.</param>
        /// <param name="workItemId">Id of the work item to delete.</param>
        /// <exception cref="T:System.ArgumentNullException">When the <paramref name="issueId"/> or <paramref name="workItemId"/> is null or empty.</exception>
        /// <exception cref="T:YouTrackErrorException">When the call to the remote YouTrack server instance returned an HTTP 400 status code, usually indicating time tracking is not enabled.</exception>
        /// <exception cref="T:System.Net.HttpRequestException">When the call to the remote YouTrack server instance failed.</exception>
        Task DeleteWorkItemForIssue(string issueId, string workItemId);
    }
}