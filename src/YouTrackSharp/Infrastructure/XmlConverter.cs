using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using YouTrackSharp.DataModel;

namespace YouTrackSharp.Infrastructure
{
    public static class XmlConverter
    {
        public static IEnumerable<Issue> ExtractIssues(string xml)
        {
            return ExtractItems(xml, "issue", ExtractIssue);
        }

        public static YouTrackUser ExtractUser(string result)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            XmlNodeList loginNodes = doc.SelectNodes("/user/@login");
            XmlNodeList fullNameNodes = doc.SelectNodes("/user/@fullName");
            if (loginNodes == null || loginNodes.Count != 1 || fullNameNodes == null || fullNameNodes.Count != 1)
                return null;

            return new YouTrackUser
                {
                    UserName = loginNodes[0].Value,
                    FullName = fullNameNodes[0].Value
                };
        }

        public static IEnumerable<YouTrackComment> ExtractComments(string xml)
        {
            return ExtractItems(xml, "comment", ExtractComment);
        }

        public static YouTrackProjectInformation ExtractProjectInformation(string xml)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            XmlElement projectElement = doc.GetElementsByTagName("project")[0] as XmlElement;
            if (projectElement == null)
                return null;

            XmlAttribute projectNameAttribute = projectElement.Attributes["name"];
            XmlAttribute shortNameAttribute = projectElement.Attributes["shortName"];
            XmlAttribute versionsAttribute = projectElement.Attributes["versions"];
            if (projectNameAttribute == null || shortNameAttribute == null)
                throw new InvalidOperationException("One of attributes is missing");
            XmlElement assigneesLoginElement = projectElement.GetElementsByTagName("assigneesLogin")[0] as XmlElement;
            XmlElement assigneesFullNameElement = projectElement.GetElementsByTagName("assigneesFullName")[0] as XmlElement;
            XmlElement subsystemsElement = projectElement.GetElementsByTagName("subsystems")[0] as XmlElement;

            List<string> assigneeLogin = new List<string>();
            if (assigneesLoginElement != null)
                foreach (XmlElement loginElement in assigneesLoginElement.GetElementsByTagName("sub"))
                    assigneeLogin.Add(loginElement.Attributes["value"].Value);

            string[] assigneeUserNames = assigneeLogin.ToArray();

            List<string> assigneeName = new List<string>();
            if (assigneesFullNameElement != null)
                foreach (XmlElement nameElement in assigneesFullNameElement.GetElementsByTagName("sub"))
                    assigneeName.Add(nameElement.Attributes["value"].Value);

            string[] assigneeFullNames = assigneeName.ToArray();
            if (assigneeFullNames.Length != assigneeUserNames.Length)
                throw new InvalidOperationException("Number of assigneeLogin and assigneeFullName tags does not match");

            List<YouTrackUser> assignees = new List<YouTrackUser>();
            for (int i = 0; i < assigneeUserNames.Length; i++)
                assignees.Add(new YouTrackUser { FullName = assigneeFullNames[i], UserName = assigneeUserNames[i] });

            List<string> versions = new List<string>();
            if (versionsAttribute != null)
                foreach (string version in versionsAttribute.Value.Substring(1, versionsAttribute.Value.Length - 1).Split(','))
                    versions.Add(version.Trim());

            List<string> subsystems = new List<string>();
            if (subsystemsElement != null)
                foreach (XmlElement sElem in subsystemsElement.GetElementsByTagName("sub"))
                    subsystems.Add(sElem.Attributes["value"].Value);

            return new YouTrackProjectInformation
                {
                    ProjectName = projectNameAttribute.Value,
                    ProjectKey = shortNameAttribute.Value,
                    Versions = versions.ToArray(),
                    Assignees = assignees.ToArray(),
                    Subsystems = subsystems.ToArray()
                };
        }

        public static IEnumerable<YouTrackFilter> ExtractFilters(string xml)
        {
            return ExtractItems(xml, "query", e => new YouTrackFilter { Name = e.Attributes["name"].Value, Query = e.Attributes["query"].Value });
        }

        public static IEnumerable<YouTrackProjectBuild> ExtractBuilds(string xml)
        {
            return ExtractItems(xml, "build", e => new YouTrackProjectBuild(e.Attributes["name"].Value, e.Attributes["assembleDate"].Value));
        }

        public static IEnumerable<T> ExtractProperties<T>(string xml, string tagName, string attributeName) where T : YouTrackNamedProperty, new()
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            return doc.GetElementsByTagName(tagName).Cast<XmlElement>().Select(tag => new T { Name = tag.Attributes[attributeName].Value });
        }

        public static IEnumerable<string> ExtractUserLogins(string result)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(result);
            XmlNodeList loginAttributes = doc.SelectNodes("/user/@login");
            if (loginAttributes == null)
                yield break;

            foreach (XmlAttribute loginAttribute in loginAttributes)
                yield return loginAttribute.Value;
        }

        private static YouTrackComment ExtractComment(XmlElement element)
        {
            XmlAttribute authorAttribute = element.Attributes["author"];
            XmlAttribute createdAttribute = element.Attributes["created"];
            XmlAttribute deletedAttribute = element.Attributes["deleted"];
            XmlAttribute issueIdAttribute = element.Attributes["issueId"];
            XmlAttribute textAttribute = element.Attributes["text"];

            return new YouTrackComment
            {
                Author = GetValue(authorAttribute),
                Created = createdAttribute != null ? ParseTime(createdAttribute.Value) : DateTime.MinValue,
                Deleted = deletedAttribute != null ? Boolean.Parse(deletedAttribute.Value) : false,
                IssueKey = GetValue(issueIdAttribute),
                Text = GetValue(textAttribute)
            };
        }


        private static Issue ExtractIssue(XmlElement issueNode)
        {
            XmlAttribute assigneeNameAttribute = issueNode.Attributes["assigneeName"];
            XmlAttribute idAttribute = issueNode.Attributes["id"];
            XmlAttribute projectShortNameAttribute = issueNode.Attributes["projectShortName"];
            XmlAttribute reporterAttribute = issueNode.Attributes["reporterName"];
            XmlAttribute priorityAttribute = issueNode.Attributes["priority"];
            XmlAttribute stateAttribute = issueNode.Attributes["state"];
            XmlAttribute typeAttribute = issueNode.Attributes["type"];
            XmlAttribute summaryAttribute = issueNode.Attributes["summary"];
            XmlAttribute descriptionAttribute = issueNode.Attributes["description"];
            XmlAttribute createdAttribute = issueNode.Attributes["created"];
            XmlAttribute updatedAttribute = issueNode.Attributes["updated"];
            XmlAttribute fixedInBuildAttribute = issueNode.Attributes["fixedInBuild"];
            XmlAttribute commentsCountAttribute = issueNode.Attributes["commentsCount"];
            XmlAttribute affectsVersionAttribute = issueNode.Attributes["affectsVersion"];
            XmlAttribute fixedVersionAttribute = issueNode.Attributes["fixedVersion"];

            string issueKey = idAttribute != null ? idAttribute.Value : null;
            int commentCount;
            Int32.TryParse(GetValue(commentsCountAttribute), out commentCount);


            List<YouTrackLink> links = new List<YouTrackLink>();
            foreach (XmlElement e in issueNode.GetElementsByTagName("issueLink"))
            {
                LinkDirection direction = e.Attributes["source"].Value == issueKey ? LinkDirection.Outward : LinkDirection.Inward;
                links.Add(new YouTrackLink
                {
                    SourceKey = e.Attributes["source"].Value,
                    DestinationKey = e.Attributes["target"].Value,
                    LinkName = e.Attributes["typeName"].Value,
                    Direction = direction,
                    DirectionDescription = direction == LinkDirection.Outward ? e.Attributes["typeOutward"].Value : e.Attributes["typeInward"].Value
                });
            }


            return new Issue
            {
                Id = issueKey,
                ProjectShortName = GetValue(projectShortNameAttribute),
                Description = GetValue(descriptionAttribute),
                Summary = GetValue(summaryAttribute),
                Assignee = GetValue(assigneeNameAttribute),
                ReporterName = GetValue(reporterAttribute),
                Priority = GetValue(priorityAttribute),
                State = GetValue(stateAttribute),
                Type = GetValue(typeAttribute),
                FixedInBuild = GetValue(fixedInBuildAttribute),
                AffectsVersion = GetValue(affectsVersionAttribute) ?? String.Empty,
                FixedVersion = GetValue(fixedVersionAttribute) ?? String.Empty,
                Links = links.ToArray(),
                CommentCount = commentCount,
                Created = createdAttribute != null ? ParseTime(createdAttribute.Value) : DateTime.MinValue,
                Updated = updatedAttribute != null ? ParseTime(updatedAttribute.Value) : DateTime.MinValue
            };
        }


        private static IEnumerable<T> ExtractItems<T>(string xml, string tagName, ItemFactory<T> factory)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(xml);
            List<T> result = new List<T>();
            foreach (XmlElement element in doc.GetElementsByTagName(tagName))
                result.Add(factory(element));

            return result.ToArray();
        }

        private static string GetValue(XmlAttribute attribute)
        {
            return attribute != null ? attribute.Value : null;
        }

        private static DateTime ParseTime(string time)
        {
            return (new DateTime(1970, 1, 1) + TimeSpan.FromMilliseconds(Int64.Parse(time))).ToLocalTime();
        }

        private static long ConvertToUnixTime(DateTime time)
        {
            return Convert.ToInt64((time.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalMilliseconds); //Unix time is time passed from 01.01.1970 in milliseconds
        }

        private delegate T ItemFactory<out T>(XmlElement element);
    }
}