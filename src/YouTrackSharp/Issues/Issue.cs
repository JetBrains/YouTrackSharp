using System;
using YouTrackSharp.DataModel;

namespace YouTrackSharp
{
    public class Issue
    {
        public string Id { get; set; }
        public string ReporterName { get; set; }
        public string FixedInBuild { get; set; }
        public string ProjectShortName { get; set; }
        public string Summary { get; set; }
        public string Assignee { get; set; }
        public string Priority { get; set; }
        public string Type { get; set; }
        public string Subsystem { get; set; }
        public string Description { get; set; }
        public string State { get; set; }
        public string AffectsVersion { get; set; }
        public string FixedVersion { get; set; }
        public YouTrackLink[] Links { get; set; }
        public DateTime Created { get; set; }
        public DateTime Updated { get; set; }
        public int CommentCount { get; set; }
    }
}