using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTrackSharp.Issues
{
    public partial class IssuesService
    {
        private const string ACTIVITIES_CATEGORIES =
            "AttachmentsCategory,CustomFieldCategory,DescriptionCategory,IssueResolvedCategory" +
            ",LinksCategory,ProjectCategory,IssueVisibilityCategory,SprintCategory,SummaryCategory,TagsCategory";
        
        /// <inheritdoc />
        public async Task<IEnumerable<Change>> GetChangeHistoryForIssue(string issueId, bool vcsHistory = false)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var categories = ACTIVITIES_CATEGORIES + (vcsHistory ? ",VcsChangeCategory" : "");

            var client = await _connection.GetAuthenticatedApiClient();
            var response = await client.IssuesActivitiesGetAsync(issueId,
                categories, false, null, null, null, Constants.FieldsQueryStrings.Activities);
            
            return response.Select(Change.FromApiEntity).ToList();
        }
    }
}