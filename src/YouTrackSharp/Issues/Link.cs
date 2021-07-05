using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using YouTrackSharp.Generated;

namespace YouTrackSharp.Issues
{
    /// <summary>
    /// Represents a <see cref="Link" /> for an <see cref="Issue" />.
    /// </summary>
    public class Link
    {
        private static string LINKS_FIELDS_QUERY = "direction,linkType(name,targetToSource,localizedTargetToSource,sourceToTarget,localizedSourceToTarget),issues(id,idReadable)";

        //TODO this doc is not appropriate
        /// <summary>
        /// Creates a <see cref="T:System.Collections.Generic.ICollection`1" /> of <see cref="Link"/> class from api client entities.
        /// </summary>
        /// <param name="entities">A <see cref="T:System.Collections.Generic.ICollection`1" /> of client entities of type <see cref="IssueLink"/> to convert from.</param>
        /// <param name="requesterIssueId">Id of issue links were requested from.</param>
        internal static ICollection<Link> FromApiEntities(ICollection<IssueLink> entities, string requesterIssueId)
        {
            
            var links = new List<Link>();
            
            foreach (var linkType in entities)
            {
                var both = linkType.LinkType.LocalizedSourceToTarget
                           ?? linkType.LinkType.LocalizedTargetToSource
                           ?? linkType.LinkType.SourceToTarget;
                var inward = linkType.Direction == IssueLinkDirection.BOTH
                    ? both
                    : linkType.LinkType.LocalizedTargetToSource ?? linkType.LinkType.TargetToSource;
                var outward = linkType.Direction == IssueLinkDirection.BOTH
                    ? both
                    : (linkType.LinkType.LocalizedSourceToTarget ?? linkType.LinkType.SourceToTarget);
                
                var linksPack = linkType.Issues.Select(issue => new Link()
                {
                    InwardType = inward,
                    OutwardType = outward,
                    TypeName = linkType.LinkType.Name,
                    Source = linkType.Direction == IssueLinkDirection.INWARD ? issue.IdReadable : requesterIssueId,
                    Target = linkType.Direction == IssueLinkDirection.INWARD ? requesterIssueId : issue.IdReadable
                });
                
                links.AddRange(linksPack.ToList());
            }

            return links;
        }

        /// <summary>
        /// Inward link type, e.g. "is required for".
        /// </summary>
        [JsonProperty("typeInward")]
        public string InwardType { get; set; }
        
        /// <summary>
        /// Outward link type, e.g. "depends on".
        /// </summary>
        [JsonProperty("typeOutward")]
        public string OutwardType { get; set; }
        
        /// <summary>
        /// Type name, e.g. "Depend".
        /// </summary>
        [JsonProperty("typeName")]
        public string TypeName { get; set; }
        
        /// <summary>
        /// Source issue.
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }
        
        /// <summary>
        /// Target issue.
        /// </summary>
        [JsonProperty("target")]
        public string Target { get; set; }
    }
}