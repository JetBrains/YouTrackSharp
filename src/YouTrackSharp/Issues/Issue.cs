using System.Collections.Generic;
using System.ComponentModel;
using JsonFx.Json;

namespace YouTrackSharp.Issues
{
    [TypeConverter(typeof(IssueTypeConverter))]
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
    }
}