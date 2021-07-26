namespace YouTrackSharp
{
    /// <summary>
    /// Constants used throughout the YouTrackSharp projects.
    /// </summary>
    public static class Constants
    {
        /// <summary>
        /// HTTP request and response content type representations.
        /// </summary>
        public static class HttpContentTypes
        {
            /// <summary>
            /// Content type for application/json.
            /// </summary>
            public const string ApplicationJson = "application/json";
        }

        /// <summary>
        /// Strings used for fields GET parameter in various API requests
        /// </summary>
        public static class FieldsQueryStrings
        {
            /// <summary>
            /// Fields query for activities resource.
            /// </summary>
            public const string Activities = "$type,added($type,agile(id),attachments($type,author(fullName,id,ringId),comment(id),created,id,imageDimensions(height,width),issue(id,project(id,ringId)),mimeType,name,removed,size,thumbnailURL,url,visibility($type,implicitPermittedUsers($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),permittedGroups($type,allUsersGroup,icon,id,name,ringId),permittedUsers($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId))),author($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),color(id),commands(end,errorText,hasError,start),comment(id),created,creator($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),date,deleted,duration(id,minutes,presentation),fetched,files,hasEmail,id,idReadable,isDraft,isLocked,issue(id,project(id,ringId)),localizedName,mimeType,minutes,name,noHubUserReason(id),noUserReason(id),numberInProject,presentation,processors($type,id),project($type,id,isDemo,leader(id),name,plugins(timeTrackingSettings(enabled,estimate(field(id,name),id),timeSpent(field(id,name),id)),vcsIntegrationSettings(processors(enabled,migrationFailed,server(enabled,url),upsourceHubResourceKey,url))),ringId,shortName),reaction,reactionOrder,reactions(author($type,fullName,id,isLocked),id,reaction),removed,reopened,resolved,shortName,size,state(id),summary,text,textPreview,thumbnailURL,type(id,name),url,urls,user($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),userName,usesMarkdown,version,visibility($type,implicitPermittedUsers($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),permittedGroups($type,allUsersGroup,icon,id,name,ringId),permittedUsers($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId))),author($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),authorGroup(icon,name),category(id),field($type,customField(fieldType(isMultiValue,valueType),id),id,linkId,presentation),id,markup,pullRequest(author($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),date,fetched,files,id,idExternal,noHubUserReason(id),noUserReason(id),textPreview,titlePreview,url,user($type,avatarUrl,email,fullName,id,isLocked,issueRelatedGroup(icon),login,name,online,profiles(general(trackOnlineStatus)),ringId),userName),removed($type,agile(id),color(id),id,idReadable,isDraft,isLocked,localizedName,name,numberInProject,project($type,id,isDemo,leader(id),name,plugins(timeTrackingSettings(enabled,estimate(field(id,name),id),timeSpent(field(id,name),id)),vcsIntegrationSettings(processors(enabled,migrationFailed,server(enabled,url),upsourceHubResourceKey,url))),ringId,shortName),reaction,resolved,state(id),summary,text),target(created,id,usesMarkdown),targetMember,targetSubMember,timestamp,type";

            /// <summary>
            /// Fields query for attachments (sub)resource.
            /// </summary>
            public const string Attachments =
                "id,url,name,author(login),visibility(permittedGroups(name)),created";

            /// <summary>
            /// Fields query for comments (sub)resource.
            /// </summary>
            public const string Comments =
                "id,author(id,login,fullName),issue(idReadable),deleted,usesMarkdown,text,textPreview,created,updated,visibility(permittedGroups(name))";

            private const string IssuesFieldDescription = "description";

            private const string IssuesFieldWikifiedDescription = "wikifiedDescription";

            /// <summary>
            /// Fields query for links (sub)resource.
            /// </summary>
            public const string IssueLinks =
                "direction,linkType(name,targetToSource,localizedTargetToSource,sourceToTarget,localizedSourceToTarget),issues(id,idReadable)";

            private const string IssuesFieldsNoDescription = "comments(" + Comments + "),links(" + IssueLinks +
                                                             "),attachments(" + Attachments +
                                                             "),id,idReadable,externalIssue(id),project(id,name,shortName),usesMarkdown,reporter(id,login,fullName),created,updated,resolved,votes,watchers(hasStar),numberInProject,updater(id,login,fullName),commentsCount,summary,tags(id,name),customFields(id,name,value(id,name,fullName,localizedName,text,markdownText,login,minutes,color(id,background,foreground))),visibility(permittedGroups(id,name))";

            /// <summary>
            /// Fields query for issues resource with issue description wikified.
            /// </summary>
            public const string IssuesWikified = IssuesFieldWikifiedDescription + "," + IssuesFieldsNoDescription;

            /// <summary>
            /// Fields query for issues resource with issue description not wikified.
            /// </summary>
            public const string IssuesNotWikified = IssuesFieldDescription + "," + IssuesFieldsNoDescription;
        }
    }
}