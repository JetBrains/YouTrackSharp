using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YouTrackSharp.Issues
{
    public partial class IssuesService
    {
        private string ACTIVITIES_CATEGORIES =
            "AttachmentsCategory,CustomFieldCategory,DescriptionCategory,IssueResolvedCategory,LinksCategory,ProjectCategory,IssueVisibilityCategory,SprintCategory,SummaryCategory,TagsCategory";
        
        private static string ACTIVITIES_FIELDS_QUERY =
            "$type,added($type,agile(id),attachments($type,author(fullName,id,ringId),comment(id),created,id,imageDimensions(height,width),issue(id,project(id,ringId)),mimeType,name,removed,size,thumbnailURL,url,visibility($type,implicitPermittedUsers($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),permittedGroups($type,allUsersGroup,icon,id,name,ringId),permittedUsers($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId))),author($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),color(id),commands(end,errorText,hasError,start),comment(id),created,creator($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),date,deleted,duration(id,minutes,presentation),fetched,files,hasEmail,id,idReadable,isDraft,isLocked,issue(id,project(id,ringId)),localizedName,mimeType,minutes,name,noHubUserReason(id),noUserReason(id),numberInProject,presentation,processors($type,id),project($type,id,isDemo,leader(id),name,plugins(timeTrackingSettings(enabled,estimate(field(id,name),id),timeSpent(field(id,name),id)),vcsIntegrationSettings(processors(enabled,migrationFailed,server(enabled,url),upsourceHubResourceKey,url))),ringId,shortName),reaction,reactionOrder,reactions(author($type,fullName,id,isLocked),id,reaction),removed,reopened,resolved,shortName,size,state(id),summary,text,textPreview,thumbnailURL,type(id,name),url,urls,user($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),userName,usesMarkdown,version,visibility($type,implicitPermittedUsers($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),permittedGroups($type,allUsersGroup,icon,id,name,ringId),permittedUsers($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId))),author($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),authorGroup(icon,name),category(id),field($type,customField(fieldType(isMultiValue,valueType),id),id,linkId,presentation),id,markup,pullRequest(author($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),date,fetched,files,id,idExternal,noHubUserReason(id),noUserReason(id),textPreview,titlePreview,url,user($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),userName),removed($type,agile(id),color(id),id,idReadable,isDraft,isLocked,localizedName,name,numberInProject,project($type,id,isDemo,leader(id),name,plugins(timeTrackingSettings(enabled,estimate(field(id,name),id),timeSpent(field(id,name),id)),vcsIntegrationSettings(processors(enabled,migrationFailed,server(enabled,url),upsourceHubResourceKey,url))),ringId,shortName),reaction,resolved,state(id),summary,text),target(created,id,usesMarkdown),targetMember,targetSubMember,timestamp,type";
        
        /// <inheritdoc />
        public async Task<IEnumerable<Change>> GetChangeHistoryForIssue(string issueId)
        {
            if (string.IsNullOrEmpty(issueId))
            {
                throw new ArgumentNullException(nameof(issueId));
            }

            var client = await _connection.GetAuthenticatedApiClient();
            var response = await client.IssuesActivitiesGetAsync(issueId,
                ACTIVITIES_CATEGORIES, false, null, null, null, ACTIVITIES_FIELDS_QUERY);
            
            return response.Select(Change.FromApiEntity).ToList();
        }
    }
}